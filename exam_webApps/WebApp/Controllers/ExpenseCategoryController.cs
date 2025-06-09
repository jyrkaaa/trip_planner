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
    public class ExpenseCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public ExpenseCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ExpenseCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExpenseCategorys.ToListAsync());
        }

        // GET: ExpenseCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategory = await _context.ExpenseCategorys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            return View(expenseCategory);
        }

        // GET: ExpenseCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ExpenseCategory expenseCategory)
        {
            if (ModelState.IsValid)
            {
                expenseCategory.Id = Guid.NewGuid();
                _context.Add(expenseCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseCategory);
        }

        // GET: ExpenseCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategory = await _context.ExpenseCategorys.FindAsync(id);
            if (expenseCategory == null)
            {
                return NotFound();
            }
            return View(expenseCategory);
        }

        // POST: ExpenseCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CategoryName,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ExpenseCategory expenseCategory)
        {
            if (id != expenseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseCategoryExists(expenseCategory.Id))
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
            return View(expenseCategory);
        }

        // GET: ExpenseCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategory = await _context.ExpenseCategorys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            return View(expenseCategory);
        }

        // POST: ExpenseCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var expenseCategory = await _context.ExpenseCategorys.FindAsync(id);
            if (expenseCategory != null)
            {
                _context.ExpenseCategorys.Remove(expenseCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseCategoryExists(Guid id)
        {
            return _context.ExpenseCategorys.Any(e => e.Id == id);
        }
    }
}
