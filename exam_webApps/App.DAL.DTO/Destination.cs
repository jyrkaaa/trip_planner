using Base.Contracts;

namespace App.DAL.DTO;

public class Destination : IDomainId
{
    public Guid Id { get; set; }
    public string CountryName { get; set; } = default!;
    public ICollection<DestinationInTrip>? DestinationsInTrip { get; set; }
}