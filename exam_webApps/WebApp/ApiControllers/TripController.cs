using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL;
using App.DTO.v1;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using DestinationInTrip = App.Domain.EF.DestinationInTrip;
using Trip = App.Domain.EF.Trip;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TripController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Trip
        [HttpGet]
    public async Task<ActionResult<IEnumerable<App.DTO.v1.Trip>>> GetTrips([FromQuery] bool includePublic = false)
    {
        var userId = User.GetUserId();

        var query = _context.Trips
            .Include(t => t.User)
            .Include(t => t!.Currency)
            .Include(t => t.DestinationsInTrip)
                .ThenInclude(d => d.Destination)
            .Include(t => t.Expenses)
                .ThenInclude(e => e.ExpenseSubCategory)
                    .ThenInclude(sc => sc!.ExpenseCategory)
            .AsQueryable();

        // Conditional filter based on parameter
        if (!includePublic)
        {
            query = query.Where(t => t.UserId == userId);
        }
        else
        {
            query = query.Where(t => t.UserId == userId || t.Public == true);
        }

        var trips = await query.ToListAsync();

        var result = trips.Select(t => new App.DTO.v1.Trip
        {
            Id = t.Id,
            BudgetOriginal = t.BudgetOriginal,
            Public = t.Public,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            UserId = t.UserId,
            BudgetBase = t.BudgetBase,
            CurrencyId = t.CurrencyId,
            Currency = new App.DTO.v1.Currency
            {
                Id = t.Currency.Id,
                Code = t.Currency.Code,
                Name = t.Currency.Name,
                Rate = t.Currency.Rate
            },
            DestinationsInTrip = [], // Or handle as needed
            Expenses = t.Expenses?.Select(e => new App.DTO.v1.Expense
            {
                Id = e.Id,
                OriginalAmount = e.OriginalAmount,
                BaseAmount = e.BaseAmount,
                ExpenseSubCategoryId = e.ExpenseSubCategoryId,
                Description = e.Description,
                ExpenseSubCategory = new App.DTO.v1.ExpenseSubCategory
                {
                    Id = e.ExpenseSubCategoryId!.Value,
                    Name = e.ExpenseSubCategory!.Name,
                    ExpenseCategory = new App.DTO.v1.ExpenseCategory
                    {
                        Id = e.ExpenseSubCategory.ExpenseCategoryId,
                        CategoryName = e.ExpenseSubCategory.ExpenseCategory.CategoryName
                    }
                }
            }).ToList()
        }).ToList();

        return result;
    }


        // GET: api/Trip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.Trip>> GetTrip(Guid id)
        {
            var userId = User.GetUserId();
            var t = await _context.Trips.
            Include(t => t.User)
            .Include(t => t.Currency)
            .Include(t => t!.DestinationsInTrip)
                .ThenInclude(d => d.Destination)
            .Include(t => t.Expenses)
                .ThenInclude(e => e.ExpenseSubCategory)
                    .ThenInclude(c => c!.ExpenseCategory)
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (t == null) return NotFound();
            var result = new App.DTO.v1.Trip
            {
                Id = t.Id,
                BudgetOriginal = t.BudgetOriginal,
                Public = t.Public,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                UserId = t.UserId,
                BudgetBase = t.BudgetBase,
                CurrencyId = t.CurrencyId,
                Currency = new App.DTO.v1.Currency { Code = t.Currency.Code, Name = t.Currency.Name, Id = t.Currency.Id, Rate = t.Currency.Rate },
                DestinationsInTrip = [],
                // DestinationsInTrip = t.DestinationsInTrip?.Select(e => new App.DTO.v1.DestinationInTrip
                // {
                //     Id = e.Id,
                //     TripId = e.TripId,
                //     DestinationsId = e.DestinationsId,
                //     Destination = new App.DTO.v1.Destination
                //     {
                //         Id = e.DestinationsId!.Value,
                //         CountryName = e.Destination!.CountryName,
                //     }
                // }).ToList(),
                Expenses = t.Expenses?.Select(e => new App.DTO.v1.Expense
                {
                    Id = e.Id,
                    OriginalAmount = e.OriginalAmount,
                    BaseAmount = e.BaseAmount,
                    ExpenseSubCategoryId = e.ExpenseSubCategoryId,
                    Description = e.Description,
                    ExpenseSubCategory = new App.DTO.v1.ExpenseSubCategory
                    {
                        Id = e.ExpenseSubCategoryId!.Value,
                        Name = e.ExpenseSubCategory!.Name,
                        ExpenseCategory = new App.DTO.v1.ExpenseCategory
                        {
                            Id = e.ExpenseSubCategory!.ExpenseCategoryId,
                            CategoryName = e.ExpenseSubCategory!.ExpenseCategory.CategoryName,
                        }
                    }
                    
                }).ToList(),
            };
            return result;
        }

        // PUT: api/Trip/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(Guid id, Trip trip)
        {
            if (id != trip.Id)
            {
                return BadRequest();
            }

            _context.Entry(trip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trip
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.Trip>> PostTrip(TripCreate trip)
        {
            var currecyId = trip.CurrencyId;
            var destinaionId = trip.DestinationId;
            
            var currencyEntiry = await _context.Currencies.FindAsync(currecyId);

            var efEntity = new Trip()
            {
                Id = Guid.NewGuid(),
                BudgetOriginal = trip.BudgetOriginal,
                Name = trip.Name,
                BudgetBase = trip.BudgetOriginal / currencyEntiry!.Rate,
                Public = trip.Public,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                CurrencyId = trip.CurrencyId,
                UserId = User.GetUserId(),
            };
            var destinationInTrip = new DestinationInTrip()
            {
                Id = Guid.NewGuid(),
                TripId = efEntity.Id,
                DestinationsId = trip.DestinationId,
            };
            
            
            _context.Trips.Add(efEntity);
            _context.DestinationInTrips.Add(destinationInTrip);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrip", new { id = efEntity.Id }, trip);
        }

        // DELETE: api/Trip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripExists(Guid id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
