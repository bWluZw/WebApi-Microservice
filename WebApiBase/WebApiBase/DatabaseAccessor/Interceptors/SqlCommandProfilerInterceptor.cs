using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace WebApiBase.DatabaseAccessor
{
    public class SqlCommandProfilerInterceptor: DbCommandInterceptor
    {
        // 读取数据之前
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            var sql = command.CommandText;
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            System.Diagnostics.Debug.WriteLine("已执行SQL语句:\r\n" + sql);
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            return base.ReaderExecuting(command, eventData, result);
        }

        // 读取数据之前（异步）
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            var sql = command.CommandText;
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            System.Diagnostics.Debug.WriteLine("已执行SQL语句（异步）:\r\n" + sql);
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");

            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        //无查询执行sql
        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            var sql = command.CommandText;
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            System.Diagnostics.Debug.WriteLine("已执行无查询SQL语句:\r\n" + sql);
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            return base.NonQueryExecuting(command, eventData, result);
        }

        // 执行 sql 返回单行单列之前
        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            var sql = command.CommandText;
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            System.Diagnostics.Debug.WriteLine("已执行返回单行单列SQL语句:\r\n" + sql);
            System.Diagnostics.Debug.WriteLine("++++++++++++++++++++");
            return base.ScalarExecuting(command, eventData, result);
        }
    }
}
