using TransportApp.Domain.Enum;

namespace TransportApp.Domain.Model;

public class BoardingEvent
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid? ReservationId { get; set; }
    public Guid PickupPointId { get; set; }

    public int SeatsBoarded { get; set; }
    public Guid ByUserId { get; set; } // driver/secretary confirming boarding
    public DateTime CreatedAt { get; set; }
    public BoardingType Type { get; set; }

    public Trip Trip { get; set; } = default!;
    public Reservation? Reservation { get; set; }
    public PickupPoint PickupPoint { get; set; } = default!;
}