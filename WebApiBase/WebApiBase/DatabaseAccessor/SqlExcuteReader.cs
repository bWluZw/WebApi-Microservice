using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using WebApiBase.Utils;
using System.Data.SqlClient;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySqlConnector;

namespace WebApiBase.DatabaseAccessor
{
    [Obsolete]
    public static class SqlExcuteReader
    {


        public static void Test(this DatabaseFacade databaseFacade, string sql, CommandType commandType = CommandType.Text)
        {
            // 判断是否是关系型数据库
            if (!databaseFacade.IsRelational()) throw new InvalidOperationException("ADO.NET仅支持关系型数据库！");

            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));
            // 检查是否支持存储过程
            //DbProvider.CheckStoredProcedureSupported(databaseFacade.ProviderName, commandType);
            // 判断是否启用 MiniProfiler 组件，如果有，则包装链接
            var dbConnection = databaseFacade.GetDbConnection();
            // 创建数据库命令对象
            var dbCommand = dbConnection.CreateCommand();
            // 设置基本参数
            dbCommand.Transaction = databaseFacade.CurrentTransaction?.GetDbTransaction();
            dbCommand.CommandType = commandType;
            //dbCommand.CommandText = realSql;
            dbCommand.CommandText = sql;
            // 设置超时
            var commandTimeout = databaseFacade.GetCommandTimeout();
            if (commandTimeout != null)
            {
                dbCommand.CommandTimeout = commandTimeout.Value;
            }
            // 判断连接字符串是否关闭，如果是，则开启
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
                // 打印数据库连接信息到 MiniProfiler
                //PrintConnectionToMiniProfiler(databaseFacade, dbConnection, false);
            }
            CommandBehavior behavior = CommandBehavior.Default;
            // 读取数据
            using var dbDataReader = dbCommand.ExecuteReader(behavior);
            // 填充到 DataTable
            var dataTable = dbDataReader.ToDataTable();
            // 释放命令对象
            dbCommand.Dispose();
        }
        /*
        public static DataTable ExecuteReader(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default)
        {

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = databaseFacade.PrepareDbCommand(sql, parameters, commandType);

            // 读取数据
            using var dbDataReader = dbCommand.ExecuteReader(behavior);

            // 填充到 DataTable
            var dataTable = dbDataReader.ToDataTable();

            // 释放命令对象
            dbCommand.Dispose();

            return dataTable;
        }

        /// <summary>
        /// 初始化数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand)</returns>
        public static (DbConnection dbConnection, DbCommand dbCommand) PrepareDbCommand(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);
            //SetDbParameters(databaseFacade.ProviderName, ref dbCommand, parameters);

            // 打开数据库连接
            OpenConnection(databaseFacade, dbConnection, dbCommand);

            // 返回
            return (dbConnection, dbCommand);
        }


        /// <summary>
        /// 创建数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand)</returns>
        private static (DbConnection dbConnection, DbCommand dbCommand) CreateDbCommand(this DatabaseFacade databaseFacade, string sql, CommandType commandType = CommandType.Text)
        {
            // 判断是否是关系型数据库
            if (!databaseFacade.IsRelational()) throw new InvalidOperationException("ADO.NET仅支持关系型数据库！");

            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

            // 检查是否支持存储过程
            //DbProvider.CheckStoredProcedureSupported(databaseFacade.ProviderName, commandType);

            // 判断是否启用 MiniProfiler 组件，如果有，则包装链接
            var dbConnection = databaseFacade.GetDbConnection();

            // 创建数据库命令对象
            var dbCommand = dbConnection.CreateCommand();

            // 设置基本参数
            dbCommand.Transaction = databaseFacade.CurrentTransaction?.GetDbTransaction();
            dbCommand.CommandType = commandType;
            //dbCommand.CommandText = realSql;
            dbCommand.CommandText = sql;

            // 设置超时
            var commandTimeout = databaseFacade.GetCommandTimeout();
            if (commandTimeout != null) dbCommand.CommandTimeout = commandTimeout.Value;

            // 返回
            return (dbConnection, dbCommand);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="dbConnection">数据库连接对象</param>
        /// <param name="dbCommand"></param>
        private static void OpenConnection(DatabaseFacade databaseFacade, DbConnection dbConnection, DbCommand dbCommand)
        {
            // 判断连接字符串是否关闭，如果是，则开启
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();

                // 打印数据库连接信息到 MiniProfiler
                //PrintConnectionToMiniProfiler(databaseFacade, dbConnection, false);
            }

            // 记录 Sql 执行命令日志
            //LogSqlExecuteCommand(databaseFacade, dbCommand);
        }*/
    }
}
