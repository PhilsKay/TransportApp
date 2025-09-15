using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration
{
    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Lat)
                .IsRequired();

            builder.Property(l => l.Lng)
                .IsRequired();

            builder.Property(l => l.Heading)
                .HasPrecision(9, 6); // optional: control precision

            builder.Property(l => l.Speed)
                .HasPrecision(9, 3); // optional: control precision

            builder.Property(l => l.CreatedAt)
                .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')")
                .IsRequired();

            // Relationship
            builder.HasOne(l => l.Trip)
                .WithMany(t => t.Locations) 
                .HasForeignKey(l => l.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
