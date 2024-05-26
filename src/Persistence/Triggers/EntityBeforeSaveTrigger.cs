using EntityFrameworkCore.Triggered;
using Svk.Domain.Common;

namespace Svk.Persistence.Triggers;

public class EntityBeforeSaveTrigger : IBeforeSaveTrigger<Entity>
{
    public Task BeforeSave(ITriggerContext<Entity> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Added)
        {
            context.Entity.CreatedAt = DateTime.UtcNow;
            context.Entity.UpdatedAt = DateTime.UtcNow;
        }

        if (context.ChangeType == ChangeType.Modified)
        {
            context.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }
}