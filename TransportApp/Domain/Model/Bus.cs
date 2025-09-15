namespace TransportApp.Domain.Model;

public class Bus
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Plate { get; set; } = default!;
    public int Capacity { get; set; }
    public bool Active { get; set; }

    // public ICollection<Trip> Trips { get; set; } = new List<Trip>();
}