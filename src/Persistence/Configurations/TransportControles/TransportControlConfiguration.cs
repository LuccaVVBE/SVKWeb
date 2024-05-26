using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Svk.Persistence.Configurations.TransportControles;

internal class TransportControlConfiguration : EntityConfiguration<TransportControl>
{
    public override void Configure(EntityTypeBuilder<TransportControl> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.LicensePlate).IsRequired();
        builder.Property(x => x.LoadingDate).IsRequired();

        builder.HasOne(x => x.Transporter)
            .WithMany()
            .IsRequired();

        builder.HasOne(x => x.Loader)
            .WithMany()
            .IsRequired();

        builder
            .HasMany(x => x.LoadingSlips)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .OwnsMany(x => x.Images, x =>
            {
                x.WithOwner().HasForeignKey("TransportControlId");
                x.Property<int>("Id");
                x.HasKey("Id");
                x.ToTable("Images");
            });

        builder.HasMany(x => x.RouteNumbers)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}