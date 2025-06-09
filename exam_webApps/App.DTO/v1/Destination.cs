using Base.Contracts;

namespace App.DTO.v1;

public class Destination : IDomainId
{
    public Guid Id { get; set; }
    public string CountryName { get; set; } = default!;
}