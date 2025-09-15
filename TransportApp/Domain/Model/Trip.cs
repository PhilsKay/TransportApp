using System.Net.NetworkInformation;
using TransportApp.Domain.Enum;

namespace TransportApp.Domain.Model;

public class Trip
{
    public Guid Id { get; set; }
    public Guid ServiceDayId { get; set; }
    public Guid BusId { get; set; }
   // public Guid? RouteId { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TripStatus Status { get; set; }
    public int CapacitySnapshot { get; set; }

    public ServiceDay ServiceDay { get; set; } = default!;
    public Bus Bus { get; set; } = default!;
    //public Route? Route { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<BoardingEvent> BoardingEvents { get; set; } = new List<BoardingEvent>();
    public ICollection<Location> Locations { get; set; } = new List<Location>();
}