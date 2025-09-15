using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration;

public class BoardingEventConfig : IEntityTypeConfiguration<BoardingEvent>
{
    public void Configure(EntityTypeBuilder<BoardingEvent> builder)
    {
        builder.ToTable("BoardingEvents");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.SeatsBoarded)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
           .HasDefaultValueSql("(NOW() AT TIME ZONE 'UTC')")
            .IsRequired();

        builder.Property(b => b.Type)
            .HasConversion<int>() // store enum as int
            .IsRequired();

        // Relationships
        builder.HasOne(b => b.Trip)
            .WithMany() 
            .HasForeignKey(b => b.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Reservation)
            .WithMany()
            .HasForeignKey(b => b.ReservationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.PickupPoint)
            .WithMany()
            .HasForeignKey(b => b.PickupPointId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
