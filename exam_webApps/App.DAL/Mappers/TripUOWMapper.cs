using App.Domain.EF;
using Base.Contracts;
using Trip = App.DAL.DTO.Trip;

namespace App.DAL.Mappers;

public class TripUOWMapper : IMapper<App.DAL.DTO.Trip, App.Domain.EF.Trip>
{
    public Trip? Map(Domain.EF.Trip? entity)
    {
        throw new NotImplementedException();
    }

    public Domain.EF.Trip? Map(Trip? entity)
    {
        throw new NotImplementedException();
    }
}