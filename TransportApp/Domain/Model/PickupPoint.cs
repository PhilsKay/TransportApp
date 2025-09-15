namespace TransportApp.Domain.Model
{
    public class PickupPoint
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double RadiusM { get; set; }
        public bool Active { get; set; }

        //public ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
        //public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        //public ICollection<BoardingEvent> BoardingEvents { get; set; } = new List<BoardingEvent>();
    }
}
