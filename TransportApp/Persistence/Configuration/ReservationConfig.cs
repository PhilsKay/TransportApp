using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration
{
    public class ReservationConfig : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.PartySize)
                .IsRequired();

            builder.Property(r => r.Status)
                .HasConversion<int>() // store enum as int
                .IsRequired();

            builder.Property(r => r.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.CreatedAt)
               .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')")
                .IsRequired();

            // Relationships
            builder.HasOne(r => r.Trip)
                .WithMany(t => t.Reservations) 
                .HasForeignKey(r => r.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(r => r.PickupPoint)
            //    .WithMany(p => p.Reservations) // PickupPoint needs ICollection<Reservation>
            //    .HasForeignKey(r => r.PickupPointId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
