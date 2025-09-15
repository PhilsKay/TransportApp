using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration
{
    public class PickupPointConfig : IEntityTypeConfiguration<PickupPoint>
    {
        public void Configure(EntityTypeBuilder<PickupPoint> builder)
        {
            builder.ToTable("PickupPoints");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Lat)
                .IsRequired();

            builder.Property(p => p.Lng)
                .IsRequired();

            builder.Property(p => p.RadiusM)
                .IsRequired();

            builder.Property(p => p.Active)
                .HasDefaultValue(true);

            //// Relationships (when you uncomment nav properties)
            //builder.HasMany<RouteStop>()
            //    .WithOne(rs => rs.PickupPoint)
            //    .HasForeignKey(rs => rs.PickupPointId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany<Reservation>()
            //    .WithOne(r => r.PickupPoint)
            //    .HasForeignKey(r => r.PickupPointId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany<BoardingEvent>()
            //    .WithOne(be => be.PickupPoint)
            //    .HasForeignKey(be => be.PickupPointId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
