using TransportApp.Domain.Enum;

namespace TransportApp.Domain.Model;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
    public Guid PickupPointId { get; set; }

    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; }
    public string Code { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Trip Trip { get; set; } = default!;
    public PickupPoint PickupPoint { get; set; } = default!;
}