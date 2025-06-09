using App.Domain.EF;
using Base.Contracts;
using Destination = App.DAL.DTO.Destination;

namespace App.DAL.Mappers;

public class DestinationUOWMapper : IMapper<App.DAL.DTO.Destination, App.Domain.EF.Destination>
{
    public Destination? Map(Domain.EF.Destination? entity)
    {
        if (entity == null) return null;
        return new Destination()
        {
            Id = entity.Id,
            CountryName = entity.CountryName,
            DestinationsInTrip = entity.DestinationsInTrip?.Select(d => new DTO.DestinationInTrip()
            {
                Id = d.Id,
                TripId = d.TripId,
                DestinationsId = d.DestinationsId,
            }).ToList(),
        };
    }

    public Domain.EF.Destination? Map(Destination? entity)
    {
        if (entity == null) return null;
        return new Domain.EF.Destination()
        {
            Id = entity.Id,
            CountryName = entity.CountryName,
            DestinationsInTrip = entity.DestinationsInTrip?.Select(d => new App.Domain.EF.DestinationInTrip()
            {
                Id = d.Id,
                TripId = d.TripId,
                DestinationsId = d.DestinationsId,
            }).ToList(),
        };
    }
}