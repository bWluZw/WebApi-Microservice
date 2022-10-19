using WebApiBase.DatabaseAccessor;

namespace WebApiBase.DatabaseAccessor
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// 数据库上下文定位器，定位器必须继承IDbContextLocator
    /// </summary>
    /// <typeparam name="IDbContextLocator1"></typeparam>
    public interface IEntity<IDbContextLocator1> where IDbContextLocator1 : class, IDbContextLocator
    {
    }
}
