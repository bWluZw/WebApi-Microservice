using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WebApiBase.Models;
using WebApiBase.Utils;

namespace WebApiBase.DatabaseAccessor
{
    public static class SqlExecuteBuilder
    {
        private static DataBaseType dbType = DataBaseType.MySql;

        #region 数据库命令处理

        //private static DbProviderFactory provider;
        /// <summary>
        /// 处理连接串和数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        private static (DbProviderFactory, string) ProviderBuilder(string connectionString, DataBaseType dataBaseType)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Build();

            if ( string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("外部数据库读取失败，请检查名称");
            }


            //string dbTypeStr = "";
            //string connectionString = "";
            //if (model.IsExtraDb)
            //{
            //    //connectionString = Configuration.GetSection("MultDbConn")[model.DbName + ":ConnectionStrings"];
            //    //dbTypeStr = Configuration.GetSection("MultDbConn")[model.DbName + ":Type"];
            //    connectionString = model.ConnectionStrings;

            //}
            //else
            //{
            //    connectionString = model.ConnectionStrings;
            //    //dbTypeStr = Enum.GetName(model.Type);
            //}

            //dbType = model.Type;
            //SymbolHandler(model.Type);
            dbType = dataBaseType;
            DbProviderFactory provider = DbProviderFactories.GetFactory(Enum.GetName(dbType));

            return (provider, connectionString);
        }

        ///// <summary>
        ///// 创建数据库命令
        ///// </summary>
        ///// <param name="provider"></param>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //public static (DbConnection dbConnection, DbCommand cmd) CreateCommand(DbProviderFactory provider, string connStr)
        //{
        //    DbConnection dbConnection = provider.CreateConnection();
        //    dbConnection.ConnectionString = connStr;
        //    DbCommand cmd = dbConnection.CreateCommand();
        //    cmd.CommandTimeout = 5;
        //    return (dbConnection, cmd);
        //}


        private static (DbConnection, DbCommand) CreateCommand(DbProviderFactory provider, string connStr, string cmdText, DbParameter[] paramsList = null, CommandType commandType = CommandType.Text)
        {
            //创建数据库连接对象
            DbConnection dbConnection = provider.CreateConnection();
            if (dbConnection == null || dbConnection.State != ConnectionState.Open)
            {
                //conn = provider.CreateConnection();
                dbConnection.ConnectionString = connStr;
                dbConnection.Open();//打开数据库连接池
            }

            //创建Command命令
            var cmd = dbConnection.CreateCommand();
            cmd.Connection = dbConnection;
            if (!string.IsNullOrEmpty(cmdText))
            {
                cmd.CommandText = cmdText;
            }
            cmd.CommandTimeout = 0;
            cmd.CommandType = commandType;
            //加载过程参数
            LoadParamter(cmd, provider, paramsList);
            return (dbConnection, cmd);
        }

        /// <summary>
        /// 加载过程参数输入至Commond中
        /// </summary>
        /// <param name="cmd"></param>
        private static void LoadParamter(DbCommand cmd, DbProviderFactory provider, DbParameter[] paramsList)
        {
            if (paramsList != null && paramsList.Count() != 0)
            {
                //if (!hasOutput && Para.Direction != ParameterDirection.Input)
                //{
                //    hasOutput = true;
                //}
                for (int i = 0; i < paramsList.Count(); i++)
                {
                    DbParameter param = CreateParameter(provider, paramsList[i].ParameterName, paramsList[i].Value, paramsList[i].DbType, paramsList[i].Direction);
                    cmd.Parameters.Add(param);
                }
            }
        }



        private static void Dispose(DbConnection conn, DbCommand cmd, DbDataAdapter dbDataAdapter = null, DbDataReader dbDataReader = null)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (dbDataAdapter != null)
            {
                dbDataAdapter.Dispose();
            }
            if (dbDataReader != null)
            {
                dbDataReader.Dispose();
            }
        }

        /// <summary>
        /// 创建过程参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="t">参数值类型</param>
        /// <param name="pDirection">参数类型</param>
        /// <returns></returns>
        private static DbParameter CreateParameter(DbProviderFactory provider, string name, object value, DbType t = DbType.Object, ParameterDirection pDirection = ParameterDirection.Input)
        {
            var para = provider.CreateParameter();
            if (t != DbType.Object)
            {
                para.DbType = t;
            }
            string placeholder = dbType == DataBaseType.Oracle ? ":" : "@";
            para.Direction = pDirection;

            if (name.StartsWith(":") || name.StartsWith("@"))
            {
                name = name.Remove(0, 1);
            }
            para.ParameterName = placeholder + name;
            para.Value = value;
            return para;
        }

        #endregion


        #region sql操作

        /// <summary>
        /// 执行SQL，并返回影响的行数
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="dataBaseType"></param>
        /// <param name="sql">SQL语句</param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public static int ExecuteSqlNonQuery(string connectionStr,DataBaseType dataBaseType, string sql, DbParameter[] paramsList = null)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, sql, paramsList);
            int count = cmd.ExecuteNonQuery();
            Dispose(dbConnection, cmd);
            return count;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="dataBaseType"></param>
        /// <param name="sql"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public static DataSet SqlQuery(string connectionStr, DataBaseType dataBaseType, string sql, DbParameter[] paramsList = null)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, sql, paramsList);


            //使用DbDataAdapter
            DbDataAdapter da = provider.CreateDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            Dispose(dbConnection, cmd, da);
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="dataBaseType"></param>
        /// <param name="sql"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public static async Task<DataSet> SqlQueryAsync(string connectionStr, DataBaseType dataBaseType, string sql, DbParameter[] paramsList = null)
        {
            var res = await Task.Run(() =>
            {
                (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
                (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, sql);
                cmd.CommandText = sql;
                DbDataAdapter da = provider.CreateDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                Dispose(dbConnection, cmd, da);
                return ds;

            });
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="dataBaseType"></param>
        /// <param name="sql"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public static int SqlExecute(string connectionStr, DataBaseType dataBaseType, string sql, DbParameter[] paramsList = null)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, sql);
            cmd.CommandText = sql;
            int rows = cmd.ExecuteNonQuery();
            Dispose(dbConnection, cmd);
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="dataBaseType"></param>
        /// <param name="sql"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public static async Task<int> SqlExecuteAsync(string connectionStr, DataBaseType dataBaseType, string sql, DbParameter[] paramsList = null)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, sql);
            cmd.CommandText = sql;
            int rows = await cmd.ExecuteNonQueryAsync();
            Dispose(dbConnection, cmd);
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="dataBaseType"></param>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public static int SqlExecuteTran(string connectionStr, DataBaseType dataBaseType, List<string> sqlList)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, "");

            DbTransaction tx = dbConnection.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                int count = 0;
                for (int n = 0; n < sqlList.Count; n++)
                {
                    string strsql = sqlList[n];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        count += cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
                Dispose(dbConnection, cmd);
                return count;
            }
            catch
            {
                tx.Rollback();
                Dispose(dbConnection, cmd);
                return 0;
            }


        }

        #region 存储过程

        /// <summary>
        /// 执行存储过程，并返回影响的行数
        /// </summary>
        /// <param name="storeProcedureName">存储过程名</param>
        /// <returns></returns>
        public static int ExecuteProceudre(string connectionStr, DataBaseType dataBaseType, string storeProcedureName, DbParameter[] dbParameters)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, storeProcedureName, dbParameters, CommandType.StoredProcedure);

            Dispose(dbConnection, cmd);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行存储过程并返回DbDataReader
        /// </summary>
        /// <param name="storeProcedureName">存储过程名</param>
        /// <returns>返回DbDataReader</returns>
        public static DataTable ExecuteProcReader(string connectionStr, DataBaseType dataBaseType, string storeProcedureName, DbParameter[] dbParameters)
        {
            (DbProviderFactory provider, string connStr) = ProviderBuilder(connectionStr, dataBaseType);
            (DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, connStr, storeProcedureName, dbParameters, CommandType.StoredProcedure);
            DbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = DataConverter.ToDataTable(dr);
            Dispose(dbConnection, cmd, null, dr);
            return dt;
        }

        #endregion


        #endregion



        ///// <summary>
        ///// 修正不同数据库命令参数前缀不一致问题
        ///// </summary>
        ///// <param name="providerName"></param>
        ///// <param name="parameterName"></param>
        ///// <param name="isFixed"></param>
        ///// <returns></returns>
        //internal static string FixSqlParameterPlaceholder(string providerName, string parameterName, DBType dbTypeParam, bool isFixed = true)
        //{
        //    var placeholder = !(DBType.Oracle == dbTypeParam) ? "@" : ":";
        //    if (parameterName.StartsWith("@") || parameterName.StartsWith(":"))
        //    {
        //        parameterName = parameterName[1..];
        //    }

        //    return isFixed ? placeholder + parameterName : parameterName;
        //}

        //public int RunProcedure(string connStr, string storedProcName, IDataParameter[] parameters, string tableName)
        //{
        //    (DbProviderFactory provider, string conn) = ProviderBuilder(dbName, connStr);
        //    //(DbConnection dbConnection, DbCommand cmd) = CreateCommand(provider, conn);
        //    DbConnection dbConnection = provider.CreateConnection();
        //    DbCommand cmd = dbConnection.CreateCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "";
        //    int count = cmd.ExecuteNonQuery();

        //    //DataSet dataSet = new DataSet();
        //    //dbConnection.Open();
        //    //DbDataAdapter sqlDA = provider.CreateDataAdapter();
        //    //sqlDA.SelectCommand = BuildQueryCommand(dbConnection, storedProcName, parameters);
        //    //sqlDA.Fill(dataSet, tableName);
        //    //sqlDA.Dispose();
        //    //dbConnection.Close();
        //    return count;
        //}
    }
}
