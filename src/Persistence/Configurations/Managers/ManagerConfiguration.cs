using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Svk.Persistence.Configurations.Managers;

internal class ManagerConfiguration : EntityConfiguration<Manager>
{
    public override void Configure(EntityTypeBuilder<Manager> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired(true);
        builder.Property(x => x.Email).IsRequired(true);
        builder.Property(x => x.Auth0Id).IsRequired(true);
    }
}