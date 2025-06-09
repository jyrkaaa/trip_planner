using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL;
using App.Domain.EF;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DestinationInTripController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DestinationInTripController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DestinationInTrip
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinationInTrip>>> GetDestinationInTrips()
        {
            return await _context.DestinationInTrips.ToListAsync();
        }

        // GET: api/DestinationInTrip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DestinationInTrip>> GetDestinationInTrip(Guid id)
        {
            var destinationInTrip = await _context.DestinationInTrips.FindAsync(id);

            if (destinationInTrip == null)
            {
                return NotFound();
            }

            return destinationInTrip;
        }

        // PUT: api/DestinationInTrip/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestinationInTrip(Guid id, DestinationInTrip destinationInTrip)
        {
            if (id != destinationInTrip.Id)
            {
                return BadRequest();
            }

            _context.Entry(destinationInTrip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationInTripExists(id))
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

        // POST: api/DestinationInTrip
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DestinationInTrip>> PostDestinationInTrip(DestinationInTrip destinationInTrip)
        {
            _context.DestinationInTrips.Add(destinationInTrip);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDestinationInTrip", new { id = destinationInTrip.Id }, destinationInTrip);
        }

        // DELETE: api/DestinationInTrip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestinationInTrip(Guid id)
        {
            var destinationInTrip = await _context.DestinationInTrips.FindAsync(id);
            if (destinationInTrip == null)
            {
                return NotFound();
            }

            _context.DestinationInTrips.Remove(destinationInTrip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DestinationInTripExists(Guid id)
        {
            return _context.DestinationInTrips.Any(e => e.Id == id);
        }
    }
}
