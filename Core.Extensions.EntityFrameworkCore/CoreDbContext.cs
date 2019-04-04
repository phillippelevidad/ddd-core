using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions options, IDomainEventDispatcher eventDispatcher) : base(options)
        {
            EventDispatcher = eventDispatcher;
        }

        protected IDomainEventDispatcher EventDispatcher { get; }

        public override int SaveChanges()
        {
            var count = base.SaveChanges();
            DispatchEventsAsync().Wait();
            return count;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var count = base.SaveChanges(acceptAllChangesOnSuccess);
            DispatchEventsAsync().Wait();
            return count;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await DispatchEventsAsync();
            return count;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = await base.SaveChangesAsync(cancellationToken);
            await DispatchEventsAsync();
            return count;
        }

        private async Task DispatchEventsAsync()
        {
            var entities = ChangeTracker.Entries<Entity>()
                .Select(e => e.Entity)
                .Where(e => e.HasDomainEvents)
                .ToArray();

            foreach (var evt in entities.SelectMany(e => e.ConsumeDomainEvents()))
                await EventDispatcher.DispatchAsync(evt);
        }
    }
}