namespace TransportApp.Domain.Model;

public class Location
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }

    public double Lat { get; set; }
    public double Lng { get; set; }
    public double? Heading { get; set; }
    public double? Speed { get; set; }
    public DateTime CreatedAt { get; set; }

    public Trip Trip { get; set; } = default!;
}
