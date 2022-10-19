using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Data.Common;
using System.Reflection;
using WebApiBase.Db;
using WebApiBase.Models;

namespace WebApiBase.DatabaseAccessor
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly BaseDbContext _context;
        //private DbSet<TEntity> Entity => _entity ?? (_entity = _context.Set<TEntity>());
        public Repository(BaseDbContext _context)
        {
            this._context = _context;
            Entities = _context.Set<TEntity>();
            DetachedEntities = Entities.AsNoTracking();
        }

        #region 属性（实体）
        /// <summary>
        /// 不追踪的实体
        /// </summary>
        public IQueryable<TEntity> DetachedEntities { get; set; }

        /// <summary>
        /// 实体
        /// </summary>
        public DbSet<TEntity> Entities { get; set; }

        #endregion


        #region 增加

        public EntityEntry<TEntity> Insert(TEntity entity)
        {
            return this.Entities.Add(entity);
        }

        public int InsertNow(TEntity entity)
        {
            this.Entities.Add(entity);
            return _context.SaveChanges();
        }

        #endregion

        #region 更新

        public int UpdateNow(TEntity entity)
        {
            this.Entities.Update(entity);
            return _context.SaveChanges();
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            return this.Entities.Update(entity);
        }

        #endregion

        #region 查询

        public DbContext GetDbContext()
        {
            return _context;
        }

        public TEntity GetModelById(int id)
        {
            return this.Entities.Find(id);
        }


        #endregion

        #region 删

        public EntityEntry<TEntity> Delete(TEntity entity)
        {
            return this._context.Remove(entity);
        }

        public int DeleteNow(TEntity entity)
        {
            this._context.Remove(entity);
            return _context.SaveChanges();
        }

        #endregion


        #region 原生sql操作

        public DataSet SqlQuery(string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(GetDbContext());
            return SqlExecuteBuilder.SqlQuery(connStr, dataBaseType, sql, paramsList);
        }

        public async Task<DataSet> SqlQueryAsync(string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(GetDbContext());
            return await SqlExecuteBuilder.SqlQueryAsync(connStr, dataBaseType, sql, paramsList);
        }

        public int SqlExecute(string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(GetDbContext());
            return SqlExecuteBuilder.SqlExecute(connStr, dataBaseType, sql, paramsList);
        }

        public async Task<int> SqlExecuteAsync(string sql, DbParameter[] paramsList = null)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(GetDbContext());
            return await SqlExecuteBuilder.SqlExecuteAsync(connStr, dataBaseType, sql, paramsList);
        }

        public int SqlExecuteTran(List<string> sqlList)
        {
            (string connStr, DataBaseType dataBaseType) = GetParam(GetDbContext());
            return SqlExecuteBuilder.SqlExecuteTran(connStr, dataBaseType, sqlList);
        }

        private (string, DataBaseType) GetParam(DbContext dbContext)
        {
            (DataBaseType dbType, _) = Utils.Utils.DbTypeHandler(dbContext.Database.ProviderName);
            string connStr = dbContext.Database.GetConnectionString();
            return (connStr, dbType);
        }

        #endregion

        public int SaveChanged()
        {
            return _context.SaveChanges();
        }
    }
}
