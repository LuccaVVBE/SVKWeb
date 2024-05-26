using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Svk.Persistence.Configurations.Transporters;

internal class TransporterConfiguration : EntityConfiguration<Transporter>
{
    public override void Configure(EntityTypeBuilder<Transporter> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired(true);
    }
}