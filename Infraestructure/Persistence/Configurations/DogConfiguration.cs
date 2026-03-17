using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="Dog"/> entity.
/// </summary>
public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(80);
        builder.Property(d => d.Breed).IsRequired().HasMaxLength(80);
        builder.Property(d => d.Age).IsRequired();
        builder.HasMany(d => d.WalkHistory)
               .WithOne()
               .HasForeignKey(w => w.DogId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(d => d.WalkHistory).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
