using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL;
using App.Domain.EF;

namespace WebApp.Controllers
{
    public class DestinationInTripController : Controller
    {
        private readonly AppDbContext _context;

        public DestinationInTripController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DestinationInTrip
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.DestinationInTrips.Include(d => d.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: DestinationInTrip/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destinationInTrip = await _context.DestinationInTrips
                .Include(d => d.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destinationInTrip == null)
            {
                return NotFound();
            }

            return View(destinationInTrip);
        }

        // GET: DestinationInTrip/Create
        public IActionResult Create()
        {
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy");
            return View();
        }

        // POST: DestinationInTrip/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripId,DestinationsId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] DestinationInTrip destinationInTrip)
        {
            if (ModelState.IsValid)
            {
                destinationInTrip.Id = Guid.NewGuid();
                _context.Add(destinationInTrip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy", destinationInTrip.TripId);
            return View(destinationInTrip);
        }

        // GET: DestinationInTrip/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destinationInTrip = await _context.DestinationInTrips.FindAsync(id);
            if (destinationInTrip == null)
            {
                return NotFound();
            }
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy", destinationInTrip.TripId);
            return View(destinationInTrip);
        }

        // POST: DestinationInTrip/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TripId,DestinationsId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] DestinationInTrip destinationInTrip)
        {
            if (id != destinationInTrip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destinationInTrip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinationInTripExists(destinationInTrip.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy", destinationInTrip.TripId);
            return View(destinationInTrip);
        }

        // GET: DestinationInTrip/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destinationInTrip = await _context.DestinationInTrips
                .Include(d => d.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destinationInTrip == null)
            {
                return NotFound();
            }

            return View(destinationInTrip);
        }

        // POST: DestinationInTrip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var destinationInTrip = await _context.DestinationInTrips.FindAsync(id);
            if (destinationInTrip != null)
            {
                _context.DestinationInTrips.Remove(destinationInTrip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinationInTripExists(Guid id)
        {
            return _context.DestinationInTrips.Any(e => e.Id == id);
        }
    }
}
