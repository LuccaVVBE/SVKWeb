using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Svk.Persistence.Configurations.Loaders;

internal class LoaderConfiguration : EntityConfiguration<Loader>
{
    public override void Configure(EntityTypeBuilder<Loader> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired(true);
        builder.Property(x => x.Email).IsRequired(true);
        builder.Property(x => x.Auth0Id).IsRequired(true);
    }
}