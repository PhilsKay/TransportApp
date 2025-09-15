using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration;

public class TripConfig : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("Trips");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.StartedAt)
            .IsRequired();

        builder.Property(t => t.EndedAt);

        builder.Property(t => t.Status)
            .HasConversion<int>() // store enum as int
            .IsRequired();

        builder.Property(t => t.CapacitySnapshot)
            .IsRequired();

        // Relationships
        builder.HasOne(t => t.ServiceDay)
            .WithMany(sd => sd.Trips)
            .HasForeignKey(t => t.ServiceDayId)
            .OnDelete(DeleteBehavior.Cascade);

        //builder.HasOne(t => t.Bus)
        //    .WithMany(b => b.Trips)
        //    .HasForeignKey(t => t.BusId)
        //    .OnDelete(DeleteBehavior.Restrict);

        // builder.HasOne(t => t.Route)
        //     .WithMany(r => r.Trips)
        //     .HasForeignKey(t => t.RouteId)
        //     .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Reservations)
            .WithOne(r => r.Trip)
            .HasForeignKey(r => r.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.BoardingEvents)
            .WithOne(be => be.Trip)
            .HasForeignKey(be => be.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Locations)
            .WithOne(l => l.Trip)
            .HasForeignKey(l => l.TripId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
