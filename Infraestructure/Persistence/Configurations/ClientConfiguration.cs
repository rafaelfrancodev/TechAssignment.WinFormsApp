using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="Client"/> entity.
/// </summary>
public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.HasMany(c => c.Dogs)
               .WithOne()
               .HasForeignKey(d => d.ClientId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.Dogs).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
