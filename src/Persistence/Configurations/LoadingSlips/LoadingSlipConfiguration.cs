using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Svk.Persistence.Configurations.LoadingSlips;

internal class LoadingSlipConfiguration : EntityConfiguration<LoadingSlip>
{
    public override void Configure(EntityTypeBuilder<LoadingSlip> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.LaadbonNr).IsRequired(true);
        builder
            .OwnsOne(x => x.LeveringsAddress, address =>
            {
                address.Property(a => a.Street).HasColumnName(nameof(Address.Street)).IsRequired();
                address.Property(a => a.Postcode).HasColumnName(nameof(Address.Postcode)).IsRequired();
                address.Property(a => a.HouseNr).HasColumnName(nameof(Address.HouseNr)).IsRequired();
                address.Property(a => a.CountryIsoCode).HasColumnName(nameof(Address.CountryIsoCode)).IsRequired();
            });
    }
}