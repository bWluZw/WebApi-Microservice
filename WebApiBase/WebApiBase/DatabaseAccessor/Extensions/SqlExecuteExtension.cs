using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WebApiBase.DatabaseAccessor;
using WebApiBase.Models;

namespace WebApiBase.Extensions
{
    [Obsolete]
    public static class SqlExecuteExtension
    {
        //private static DbInfoModel GetParam(DbContext dbContext)
        //{
        //    DbInfoModel model = new DbInfoModel();
        //    (DataBaseType dbType, _) = Utils.Utils.DbTypeHandler(dbContext.Database.ProviderName);
        //    model.Type = dbType;
        //    model.IsExtraDb = false;
        //    model.ConnectionStrings = dbContext.Database.GetConnectionString();

        //    return model;
        //}

        private static (string, DataBaseType) GetParam(DbContext dbContext)
        {
            (DataBaseType dbType, _) = Utils.Utils.DbTypeHandler(dbContext.Database.ProviderName);
            string connStr = dbContext.Database.GetConnectionString();

            return (connStr, dbType);
        }

        public static DataSet SqlQuery(this DbContext dbContext, string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(dbContext);
            return SqlExecuteBuilder.SqlQuery(connStr, dataBaseType, sql, paramsList);
        }

        public static async Task<DataSet> SqlQueryAsync(this DbContext dbContext, string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(dbContext);
            return await SqlExecuteBuilder.SqlQueryAsync(connStr, dataBaseType, sql, paramsList);
        }

        public static int SqlExecute(this DbContext dbContext, string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(dbContext);
            return SqlExecuteBuilder.SqlExecute(connStr, dataBaseType, sql, paramsList);
        }

        public async static Task<int> SqlExecuteAsync(this DbContext dbContext, string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(dbContext);
            return await SqlExecuteBuilder.SqlExecuteAsync(connStr, dataBaseType, sql, paramsList);
        }

        public static int SqlExecuteTran(this DbContext dbContext, List<string> sqlList)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(dbContext);
            return SqlExecuteBuilder.SqlExecuteTran(connStr, dataBaseType, sqlList);
        }
    }
}
