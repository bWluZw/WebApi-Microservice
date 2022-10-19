using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Data.Common;
using WebApiBase.Models;

namespace WebApiBase.DatabaseAccessor
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public EntityEntry<TEntity> Insert(TEntity entity);
        public int InsertNow(TEntity entity);

        /// <summary>
        /// 更新一个实体，立即执行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateNow(TEntity entity);

        /// <summary>
        /// 删除一个实体，等待SaveChanged
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public EntityEntry<TEntity> Delete(TEntity entity);

        /// <summary>
        /// 删除一个实体，立即执行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteNow(TEntity entity);

        /// <summary>
        /// 根据id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetModelById(int id);

        #region Sql操作
        /// <summary>
        /// sql查询，返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataSet SqlQuery(string sql, DbParameter[] param);

        /// <summary>
        /// 异步sql查询，返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<DataSet> SqlQueryAsync(string sql, DbParameter[] param);

        /// <summary>
        /// 执行增删改，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int SqlExecute(string sql, DbParameter[] param);

        /// <summary>
        /// 异步执行增删改，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<int> SqlExecuteAsync(string sql, DbParameter[] param);

        /// <summary>
        /// 事务执行
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public int SqlExecuteTran(List<string> sqlList);

        #endregion

        #region Other
        public DbContext GetDbContext();

        ///// <summary>
        ///// 执行插入前的操作，例如添加创建时间等
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public TEntity PreInsertHandler(TEntity entity);

        ///// <summary>
        ///// 执行更新前的操作，例如更改修改时间等
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public TEntity PreUpdateHandler(TEntity entity);

        /// <summary>
        /// 立即保存当前对实体的操作，返回受影响的行数
        /// </summary>
        /// <returns></returns>
        public int SaveChanged();

        /// <summary>
        /// 追踪的实体
        /// </summary>
        public DbSet<TEntity> Entities { get; set; }

        /// <summary>
        /// 不追踪的实体
        /// </summary>
        public IQueryable<TEntity> DetachedEntities { get; set; }

        #endregion


    }
}
