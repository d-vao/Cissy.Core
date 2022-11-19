using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;


namespace Cissy.Tools.Templates
{
    public class DatabaseSchemaManager
    {
        public DatabaseSchemaManager(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        protected string ConnectionString;
        //判断表是否存在
        // Select * From sysObjects Where Name ='表名' And Type In ('S','U')
        public bool TableExists(string TableName)
        {
            string sql = string.Format("Select count(*) From sysObjects Where Name ='{0}' And Type In ('S','U')", TableName);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        //判断表字段是否存在
        //select * from syscolumns where id=object_id('表名') and name='字段名'
        public bool TableColumnExists(string TableName, string ColumnName)
        {
            string sql = string.Format("select count(*) from syscolumns where id=object_id('{0}') and name='{1}'", TableName, ColumnName);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public void CreateTable(string TableName, ColumnMap[] columns, string PrimaryKeyName)
        {
            List<string> list = new List<string>();
            foreach (ColumnMap colMap in columns)
            {
                string s = string.Empty;
                if (colMap.ColumnName != PrimaryKeyName)
                {
                    s = string.Format("[{0}]  {1} null", colMap.ColumnName, colMap.DbType);
                }
                else
                {
                    s = string.Format("[{0}]  {1} NOT NULL primary key", colMap.ColumnName, colMap.DbType);
                }
                list.Add(s);
            }
            string cols = string.Join(",", list.ToArray());
            string sql = string.Format("CREATE TABLE [dbo].[{0}]({1})", TableName, cols);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void CreateColumn(string TableName, string ColumnName, string SqlDbType)
        {
            string sql = string.Format("ALTER TABLE {0} ADD {1} {2} NULL", TableName, ColumnName, SqlDbType);
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
