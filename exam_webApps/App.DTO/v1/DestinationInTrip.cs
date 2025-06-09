using Base.Contracts;

namespace App.DTO.v1;

public class DestinationInTrip : IDomainId
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; } = default!;
    public Trip Trip { get; set; } = default!;

    public Guid? DestinationsId { get; set; }
    public Destination? Destination { get; set; }
}