using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Svk.Domain.Common;

namespace Svk.Persistence.Configurations;

class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(typeof(T).Name);
        builder.Property(x => x.IsEnabled).IsRequired().HasDefaultValue(true).ValueGeneratedNever();
        builder.Property(x => x.UpdatedAt).IsConcurrencyToken();
    }
}