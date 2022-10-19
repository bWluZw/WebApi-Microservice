

using Microsoft.Extensions.Configuration.Json;
using System.Collections;
using System.Data;
using System.Data.Common;
using WebApiBase.Models;

namespace WebApiBase.DatabaseAccessor
{
    [Obsolete]
    public class SqlHelper
    {
        #region 成员定义
        ////public string ConnStr { get; set; }
        //private DbProviderFactory dbProvider;//数据库Provider
        //private DBType dbType;//数据库类型
        private char pSymbol = '@';//参数符号
        //private string providerName;

        //private string connectionString;//连接字符串
        //private DbConnection conn;//连接对象
        //private DbTransaction tran;//事务对象

        //private IList parameterList = new List<DbParameter>();//过程参数列表
        //private bool hasOutput = false;//是否包含输出参数
        //private Dictionary<string, object> dicPara = new Dictionary<string, object>();//输出参数列表
        #endregion




        //private IConfiguration _config;

        /// <summary>
        /// 读取WebConfig链接字符串
        /// </summary>
        /// <param name="connectionName">ConnectionString配置名</param>
        public SqlHelper(string connectionName = "Db1")
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Build();
            string connectionString = Configuration.GetSection("MultDbConn")[connectionName + ":ConnectionStrings"];
            string dbType = Configuration.GetSection("MultDbConn")[connectionName + ":Type"];


            (DataBaseType dBType,string providerName) = DbTypeHandler(dbType);
            DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);

            CreateCommandTest(provider, connectionString);
        }

        public (DbConnection dbConnection, DbCommand cmd) CreateCommand(DbProviderFactory provider, string connStr)
        {
            using (DbConnection dbConnection = provider.CreateConnection())
            {
                dbConnection.ConnectionString = connStr;
                dbConnection.Open();
                using (DbCommand cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandTimeout = 5;
                    return (dbConnection, cmd);
                }
            }
        }

        //public (DbConnection dbConnection, DbCommand cmd) SetParam(DbConnection dbConnection, DbCommand cmd)
        //{ 
            
        //}

        public dynamic CreateCommandTest(DbProviderFactory provider, string connStr)
        {
            using (DbConnection dbConnection = provider.CreateConnection())
            {
                dbConnection.ConnectionString = connStr;
                dbConnection.Open();
                DbCommand cmd = dbConnection.CreateCommand();
                cmd.CommandText = "select * from user";
                //使用DbDataAdapter
                DbDataAdapter da = provider.CreateDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                System.Diagnostics.Debug.WriteLine(ds.Tables[0].Rows[0]["Pwd"]);

                //使用DbDataReader
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    System.Diagnostics.Debug.WriteLine(reader.ToString());
                }
                dbConnection.Close();
            }
            return "";
        }

        //public SqlHelper(string strConn)
        //{
        //    dbProvider = DbProviderFactories.GetFactory(providerType);
        //    connectionString = strConn;
        //    DbTypeHandler("");
        //}

        private (DataBaseType dbType, string providerName) DbTypeHandler(string strType)
        {
            DataBaseType dbType = DataBaseType.MySql;
            string providerName = "";
            if (strType.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                dbType = DataBaseType.Oracle;
                providerName = "Oracle";
            }
            else if (strType.IndexOf("SqlServer", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                dbType = DataBaseType.SqlServer;
                providerName = "SqlServer";
            }
            else if (strType.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                dbType = DataBaseType.MySql;
                providerName = "MySql";
            }

            if (dbType == DataBaseType.Oracle)
                pSymbol = ':';
            else
                pSymbol = '@';
            return (dbType, providerName);
        }



        //private static (DbConnection dbConnection, DbCommand dbCommand) CreateDbCommand(string sql, CommandType commandType = CommandType.Text)
        //{
        //    ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["default"];
        //    DbProviderFactory provider = DbProviderFactories.GetFactory(settings.ProviderName);
        //}

    }
}
