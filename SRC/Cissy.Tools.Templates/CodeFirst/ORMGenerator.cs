using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
//using EnvDTE;
//using ESF.Common.Core;
//using ESF.Common.Data;

namespace Cissy.Tools.Templates
{
    public class ORMGenerator
    {
        public List<string> Fs { get; set; }
        public ITextTemplatingEngineHost Host { get; set; }
        public string SectionName { get; private set; }
        public string ConnectionName { get; private set; }
        public string SchemaConnectionString { get; private set; }
        //ProjectItem ProjectItem;
        public ORMGenerator(ITextTemplatingEngineHost host, string SectionName, string ConnectionName, string SchemaConnectionString)
        {
            this.Host = host;
            this.SectionName = SectionName;
            this.ConnectionName = ConnectionName;
            this.SchemaConnectionString = SchemaConnectionString;
            this.Fs = new List<string>();
            //DTE Dte = (DTE)((IServiceProvider)host).GetService(typeof(DTE));
            //ProjectItem = Dte.Solution.FindProjectItem(this.Host.TemplateFile);
        }
        Dictionary<Type, ORM> ORMs = new Dictionary<Type, ORM>();
        protected IDictionary<string, Template> CreateEntityTemplates()
        {
            Dictionary<string, Template> templates = new Dictionary<string, Template>();
            foreach (ORM orm in ORMs.Values)
            {
                templates.Add(string.Format("{0}-Repository.cs", orm.EntityType.Name), new EntityMapTemplate(orm.EntityType, this.SectionName, orm.TableName, orm.KeyField, orm.Columns));
            }
            return templates;
        }
        protected IDictionary<string, Template> CreateProviderTemplates()
        {
            Dictionary<string, Template> templates = new Dictionary<string, Template>();
            templates.Add(string.Format("{0}-Provider.cs", this.SectionName), new EntityProviderTemplate(SectionName, this.ConnectionName, ORMs));
            return templates;
        }

        public string Run()
        {
            this.CreateEntityMap();
            return this.CreateProviderMap();
        }
        void CreateEntityMap()
        {
            string directorybase = Path.GetDirectoryName(this.Host.TemplateFile);
            string directory = Path.Combine(directorybase, string.Format("{0}_Maps", this.SectionName));
            string directory2 = Path.Combine(directorybase, string.Format("{0}_MapExts", this.SectionName));

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!Directory.Exists(directory2))
            {
                Directory.CreateDirectory(directory2);
            }
            foreach (var item in this.CreateEntityTemplates())
            {
                item.Value.TransformText();
                string code = item.Value.ToCode();
                CreateFile(this.Host, Path.Combine(directory, item.Key), code);
            }
        }
        string CreateProviderMap()
        {
            string directory = Path.GetDirectoryName(this.Host.TemplateFile);
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.CreateProviderTemplates())
            {
                item.Value.TransformText();
                string code = item.Value.ToCode();
                sb.Append(code);
                sb.Append("\n");
            }
            return sb.ToString();
        }
        void CreateFile(ITextTemplatingEngineHost host, string fileName, string contents)
        {
            if (File.Exists(fileName) && File.ReadAllText(fileName) == contents)
            {
                return;
            }
            File.WriteAllText(fileName, contents);
            this.Fs.Add(fileName);
            //DTE Dte = (DTE)((IServiceProvider)host).GetService(typeof(DTE));
            //ProjectItem ProjectItem = Dte.Solution.FindProjectItem(this.Host.TemplateFile);
            //if (ProjectItem.ProjectItems.Cast<ProjectItem>().Any(item => item.get_FileNames(0) != fileName))
            //{
            //    ProjectItem.ProjectItems.AddFromFile(fileName);
            //}
        }
        public ORMGenerator RegisterMapBuilder(IMapBuilder Builder)
        {
            DatabaseSchemaManager manager = null;
            try
            {
                manager = new DatabaseSchemaManager(SchemaConnectionString);
            }
            catch
            {

            }
            foreach (ORM m in Builder.Build())
            {
                ORMs[m.EntityType] = m;
                //构建架构
                if (manager != null)
                {
                    try
                    {
                        if (!m.IsView)
                        {
                            if (manager.TableExists(m.TableName))
                            {
                                foreach (ColumnMap col in m.Columns.Values)
                                {
                                    if (!manager.TableColumnExists(m.TableName, col.ColumnName))
                                    {
                                        manager.CreateColumn(m.TableName, col.ColumnName, col.DbType);
                                    }
                                }
                            }
                            else
                            {
                                manager.CreateTable(m.TableName, m.Columns.Values.ToArray(), m.KeyField);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return this;
        }


    }
}
