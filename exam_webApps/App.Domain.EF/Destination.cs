using Base.Domain;

namespace App.Domain.EF;

public class Destination : BaseEntity
{
    public string CountryName { get; set; } = default!;
    public ICollection<DestinationInTrip>? DestinationsInTrip { get; set; }
}