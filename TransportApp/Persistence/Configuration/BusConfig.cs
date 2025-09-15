using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportApp.Domain.Model;

namespace TransportApp.Persistence.Configuration
{
    public class BusConfig : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.ToTable("Buses");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.Plate)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.Capacity)
                .IsRequired();

            builder.Property(b => b.Active)
                .HasDefaultValue(true);

            // Relationships
            //builder.HasMany<Trip>()
            //    .WithOne(t => t.Bus)
            //    .HasForeignKey(t => t.BusId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
