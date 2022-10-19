using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApiBase.DatabaseAccessor
{
    public class DbContextSaveChangesInterceptor: SaveChangesInterceptor
    {
        // 提交到数据库之后
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return base.SavedChanges(eventData, result);
        }
        // 提交到数据库之后（异步）
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
