using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration;

public class ServiceDayConfig : IEntityTypeConfiguration<ServiceDay>
{
    public void Configure(EntityTypeBuilder<ServiceDay> builder)
    {
        builder.ToTable("ServiceDays");

        builder.HasKey(sd => sd.Id);

        builder.Property(sd => sd.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(sd => sd.StartAt)
            .IsRequired();

        builder.Property(sd => sd.Description)
            .HasMaxLength(3000);

        builder.Property(sd => sd.ImageUrl)
            .HasMaxLength(300);

        // Relationships
        builder.HasMany(sd => sd.Trips)
            .WithOne(t => t.ServiceDay)
            .HasForeignKey(t => t.ServiceDayId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
