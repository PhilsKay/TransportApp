namespace TransportApp.Domain.Model;

public class ServiceDay
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime StartAt { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
