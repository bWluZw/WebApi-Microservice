using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging.Debug;
using WebApiBase.Extensions;
using WebApiBase.Models;

namespace WebApiBase.Db
{
    public class BaseDbContext : DbContext
    {
        //public static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
        //string _connectionString = string.Empty;
        //string _providerType = string.Empty;
        //public BaseDbContext(string connectionString, string providerType)
        //{
        //    _connectionString = connectionString;
        //    _providerType = providerType;
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    if ("Npgsql.EntityFrameworkCore.PostgreSQL" == _providerType)
            //        optionsBuilder.UseNpgsql(_connectionString);
            //    else if ("Microsoft.EntityFrameworkCore.SqlServer" == _providerType)
            //        optionsBuilder.UseSqlServer(_connectionString);
            //    else if ("Microsoft.EntityFrameworkCore.Sqlite" == _providerType)
            //        optionsBuilder.UseSqlite(_connectionString);
            //    else
            //        throw new ApplicationException("未知的数据库 Provider");
            //}

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
            modelBuilder.RegisterEntities(assembly);

            base.OnModelCreating(modelBuilder);
        }

        public BaseDbContext(DbContextOptions<BaseDbContext> option) : base(option)
        {
            //DBDatabase = this.Database;
            //string conn = DBDatabase.GetConnectionString();
        }
    }
}

