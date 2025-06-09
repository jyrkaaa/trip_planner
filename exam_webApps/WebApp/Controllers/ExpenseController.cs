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
    public class ExpenseController : Controller
    {
        private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Expense
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Expenses.Include(e => e.ExpenseSubCategory).Include(e => e.Trip);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Expense/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.ExpenseSubCategory)
                .Include(e => e.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expense/Create
        public IActionResult Create()
        {
            ViewData["ExpenseCategoryId"] = new SelectList(_context.ExpenseCategorys, "Id", "CategoryName");
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy");
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,Description,ExpenseCategoryId,TripId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                expense.Id = Guid.NewGuid();
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseCategoryId"] = new SelectList(_context.ExpenseCategorys, "Id", "CategoryName", expense.ExpenseSubCategoryId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy", expense.TripId);
            return View(expense);
        }

        // GET: Expense/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["ExpenseCategoryId"] = new SelectList(_context.ExpenseCategorys, "Id", "CategoryName", expense.ExpenseSubCategoryId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy", expense.TripId);
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Amount,Description,ExpenseCategoryId,TripId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
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
            ViewData["ExpenseCategoryId"] = new SelectList(_context.ExpenseCategorys, "Id", "CategoryName", expense.ExpenseSubCategoryId);
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "CreatedBy", expense.TripId);
            return View(expense);
        }

        // GET: Expense/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.ExpenseSubCategory)
                .Include(e => e.Trip)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(Guid id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
