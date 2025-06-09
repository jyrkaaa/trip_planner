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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Destination = App.Domain.EF.Destination;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DestinationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DestinationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Destination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.Destination>>> GetDestinations()
        {
            var list = await _context.Destinations.ToListAsync();
            return list.Select(d => new App.DTO.v1.Destination()
            {
                Id = d.Id,
                CountryName = d.CountryName,
            }).ToList();
        }

        // GET: api/Destination/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.Destination>> GetDestination(Guid id)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }

            return new App.DTO.v1.Destination() {Id = destination.Id, CountryName = destination.CountryName};
        }

        // PUT: api/Destination/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(Guid id, Destination destination)
        {
            if (id != destination.Id)
            {
                return BadRequest();
            }

            _context.Entry(destination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
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

        // POST: api/Destination
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.Destination>> PostDestination(DestinationCreate destination)
        {
            var efEntity = new Destination()
            {
                CountryName = destination.CountryName,
                Id = Guid.NewGuid(),
            };
            _context.Destinations.Add(efEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDestination", new { id = efEntity.Id }, destination);
        }

        // DELETE: api/Destination/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(Guid id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DestinationExists(Guid id)
        {
            return _context.Destinations.Any(e => e.Id == id);
        }
    }
}
