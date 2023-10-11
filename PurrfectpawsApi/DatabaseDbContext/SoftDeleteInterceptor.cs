using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PurrfectpawsApi.Interfaces;

namespace PurrfectpawsApi.DatabaseDbContext
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context is null) return ValueTask.FromResult(result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;

                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
            }
            return ValueTask.FromResult(result);
        }

    }
}
