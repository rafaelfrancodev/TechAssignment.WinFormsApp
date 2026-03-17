using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="WalkEvent"/> entity.
/// </summary>
public class WalkEventConfiguration : IEntityTypeConfiguration<WalkEvent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<WalkEvent> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.WalkDate).IsRequired();
        builder.Property(w => w.DurationMinutes).IsRequired();
        builder.Property(w => w.Notes).HasMaxLength(500);
    }
}
