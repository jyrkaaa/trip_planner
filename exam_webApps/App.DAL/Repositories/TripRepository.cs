using App.DAL.Contracts;
using App.DAL.Mappers;
using App.Domain.EF;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace App.DAL.Repositories;

public class TripRepository : BaseRepository<DTO.Trip, App.Domain.EF.Trip> , ITripRepository
{
    public TripRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new TripUOWMapper())
    {
    }

    public IEnumerable<DTO.Trip> All(Guid userId = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DTO.Trip>> AllAsync(Guid userId = default)
    {
        throw new NotImplementedException();

        // var query = GetQuery()
        //     .Include(c => c.Exercises);
        // return (await query.ToListAsync()).Select(c => Mapper.Map(c!));;
    }

    public DTO.Trip? Find(Guid id, Guid userId = default)
    {
        throw new NotImplementedException();
    }

    public async Task<DTO.Trip?> FindAsync(Guid id, Guid userId = default)
    {
        throw new NotImplementedException();

        // var query = await GetQuery()
        //     .Include(e => e.Exercises)
        //     .FirstOrDefaultAsync(e => e.Id == id);
        // return Mapper.Map(query);
    }

    public override void Add(DTO.Trip entity, Guid userId = default)
    {
        var efEntity = Mapper.Map(entity);
        if (efEntity == null) return;
        
        RepositoryDbSet.Add(efEntity);
        
    }

    public DTO.Trip Update(Trip entity)
    {
        throw new NotImplementedException();
    }

    public void Remove(Trip entity, Guid userId = default)
    {
        throw new NotImplementedException();
    }
}