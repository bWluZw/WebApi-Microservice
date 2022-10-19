using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace WebApiBase.DatabaseAccessor
{
    public class SqlConnectionInterceptor: DbConnectionInterceptor
    {
        // 数据库连接关闭之前
        public override InterceptionResult ConnectionClosing(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            //eventData.StartTime
            return base.ConnectionClosing(connection, eventData, result);
        }
        // 数据库连接关闭之前（异步）
        public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            return base.ConnectionClosingAsync(connection, eventData, result);
        }
    }
}
