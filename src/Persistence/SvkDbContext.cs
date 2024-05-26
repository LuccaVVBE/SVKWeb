using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Svk.Persistence.Triggers;

namespace Svk.Persistence;

public class SvkDbContext : DbContext
{
    public SvkDbContext(DbContextOptions<SvkDbContext> options)
        : base(options)
    {
    }

    public DbSet<TransportControl> TransportControls => Set<TransportControl>();
    public DbSet<Transporter> Transporters => Set<Transporter>();
    public DbSet<LoadingSlip> LoadingSlips => Set<LoadingSlip>();
    public DbSet<Loader> Loaders => Set<Loader>();
    public DbSet<Manager> Managers => Set<Manager>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseTriggers(options => { options.AddTrigger<EntityBeforeSaveTrigger>(); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}