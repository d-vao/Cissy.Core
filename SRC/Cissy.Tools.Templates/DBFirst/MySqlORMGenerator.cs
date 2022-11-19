using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
//using EnvDTE;

namespace Cissy.Tools.Templates
{
    public class MySqlORMGenerator
    {
        public ITextTemplatingEngineHost Host { get; set; }
        public string SchemaConnectionString { get; private set; }
        public string Namespaces { get; private set; }
        protected MySqlMetaData meta { get; set; }
        public MySqlORMGenerator(ITextTemplatingEngineHost host, string namespaces, string schemaConnectionString)
        {
            this.Host = host;
            this.SchemaConnectionString = schemaConnectionString;
            this.Namespaces = namespaces;
            meta = new MySqlMetaData(this.SchemaConnectionString, this.Namespaces);
        }
        public MySqlORMGenerator(ITextTemplatingEngineHost host, string namespaces, Uri ConfigUrl)
        {
            string connection = string.Empty;
            HttpHelper.Get(ConfigUrl.ToString(), m =>
            {
                connection = m;
            });
            this.Host = host;
            this.SchemaConnectionString = connection;
            this.Namespaces = namespaces;
            meta = new MySqlMetaData(this.SchemaConnectionString, this.Namespaces);
        }
        public void Run()
        {
            meta.Init();
            CreateDataModels();
        }
        void CreateFile(ITextTemplatingEngineHost host, string fileName, string contents)
        {
            if (File.Exists(fileName) && File.ReadAllText(fileName) == contents)
            {
                return;
            }
            File.WriteAllText(fileName, contents);
            //this.Fs.Add(fileName);
            //DTE Dte = (DTE)((IServiceProvider)host).GetService(typeof(DTE));
            //ProjectItem ProjectItem = Dte.Solution.FindProjectItem(this.Host.TemplateFile);
            //if (ProjectItem.ProjectItems.Cast<ProjectItem>().Any(item => item.get_FileNames(0) != fileName))
            //{
            //    ProjectItem.ProjectItems.AddFromFile(fileName);
            //}
        }
        void CreateDataModels()
        {
            string directorybase = Path.GetDirectoryName(this.Host.TemplateFile);
            string directory = Path.Combine(directorybase, "Cissy_Data_Models");
            string directory2 = Path.Combine(directorybase, "Cissy_Data_ModelExtensions");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!Directory.Exists(directory2))
            {
                Directory.CreateDirectory(directory2);
            }
            foreach (var item in this.CreateDataModelTemplates())
            {
                item.Value.TransformText();
                string code = item.Value.ToCode();
                CreateFile(this.Host, Path.Combine(directory, item.Key), code);
            }
        }
        protected IDictionary<string, Template> CreateDataModelTemplates()
        {
            Dictionary<string, Template> templates = new Dictionary<string, Template>();
            foreach (Table table in meta.Tables.Values)
            {
                templates.Add(string.Format("{0}-Model.cs", table.TableName), new MySqlDataModelTemplate(table));
            }
            return templates;
        }

    }
}
