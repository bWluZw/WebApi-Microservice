using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiBase.DatabaseAccessor;
using WebApiBase.Utils;

namespace WebApiBase.Extensions
{
    [Obsolete]
    public static class SqlExecuteHandlerExtension
    {
        /*
        /// <summary>
        /// sql同步查询，返回DataSet
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataSet SqlQuery(this DbContext dbContext, string sql)
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            return SqlExecuteHelper.Query(conn, sql);
        }

        /// <summary>
        /// sql异步查询，返回DataSet
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<DataSet> SqlQueryAsync(this DbContext dbContext, string sql)
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }

            var res = await Task.Run(() =>
            {
                return SqlExecuteHelper.Query(conn, sql);
            });
            return res;
        }

        /// <summary>
        /// sql同步查询，返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<T> SqlQuery<T>(this DbContext dbContext, string sql) where T : new()
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            DataSet oriData = SqlExecuteHelper.Query(conn, sql);
            if (oriData.Tables.Count <= 0)
            {
                return new List<T>();
            }
            //List<T> list = (from r in oriData.Tables[0].AsEnumerable() select r.Field<T>("列名")).ToList<T>();
            var list = DataConverter.TableToList<T>(oriData.Tables[0]);
            return list;
        }

        /// <summary>
        /// sql异步查询，返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<List<T>> SqlQueryAsync<T>(this DbContext dbContext, string sql) where T : new()
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            var res = await Task.Run(() =>
            {
                DataSet oriData = SqlExecuteHelper.Query(conn, sql);
                if (oriData.Tables.Count <= 0)
                {
                    return new List<T>();
                }
                //List<T> list = (from r in oriData.Tables[0].AsEnumerable() select r.Field<T>("列名")).ToList<T>();
                var list = DataConverter.TableToList<T>(oriData.Tables[0]);
                return list;
            });

            return res;
        }

        public static T SqlQuerySingle<T>(this DbContext dbContext, string sql) where T : class, new()
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            var oriData = SqlExecuteHelper.GetSingle(conn, sql);
            T data = oriData as T;
            return data;
        }

        public static async Task<T> SqlQuerySingleAsync<T>(this DbContext dbContext, string sql) where T : class, new()
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            var res = await Task.Run(() =>
            {
                var oriData = SqlExecuteHelper.GetSingle(conn, sql);
                return oriData;
            });

            T data = res as T;
            return data;
        }

        /// <summary>
        /// 同步执行sql增删改操作，返回受影响的行数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int SqlExecute(this DbContext dbContext, string sql)
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }

            return SqlExecuteHelper.ExecuteSql(conn, sql);
        }


        public static async Task<int> SqlExecuteAsync(this DbContext dbContext, string sql)
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            var res = await Task.Run(() =>
            {
                return SqlExecuteHelper.ExecuteSql(conn, sql);
            });
            return res;
        }

        /// <summary>
        /// 同步执行有事务控制的sql增删改
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int SqlExecuteTransaction(this DbContext dbContext, List<string> sqlList)
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            return SqlExecuteHelper.ExecuteSqlTran(conn, sqlList);
        }

        public static async Task<int> SqlExecuteTransactionAsync(this DbContext dbContext, List<string> sqlList)
        {
            string conn = dbContext.Database.GetConnectionString();
            if (string.IsNullOrEmpty(conn))
            {
                throw new Exception("数据库连接字符串不能为空");
            }
            var res = await Task.Run(() =>
            {
                return SqlExecuteHelper.ExecuteSqlTran(conn, sqlList);
            });
            return res;
        }

        /// <summary>
        /// 非追踪的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IQueryable<T> DetachedEntities<T>(this DbContext dbContext) where T : class
        {
            var Entities = dbContext.Set<T>();
            return Entities.AsNoTracking();
        }*/
    }
}
