using App.Domain.EF.Identity;
using Base.Contracts;
using Base.Domain;

namespace App.Domain.EF;

public class DestinationInTrip : BaseEntity
{

    public Guid TripId { get; set; } = default!;
    public Trip Trip { get; set; } = default!;

    public Guid? DestinationsId { get; set; }
    public Destination? Destination { get; set; }
}
