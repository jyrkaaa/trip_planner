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
    public class ExpenseSubCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public ExpenseSubCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ExpenseSubCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExpenseSubCategorys.ToListAsync());
        }

        // GET: ExpenseSubCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseSubCategory = await _context.ExpenseSubCategorys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseSubCategory == null)
            {
                return NotFound();
            }

            return View(expenseSubCategory);
        }

        // GET: ExpenseSubCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseSubCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ExpenseSubCategory expenseSubCategory)
        {
            if (ModelState.IsValid)
            {
                expenseSubCategory.Id = Guid.NewGuid();
                _context.Add(expenseSubCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseSubCategory);
        }

        // GET: ExpenseSubCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseSubCategory = await _context.ExpenseSubCategorys.FindAsync(id);
            if (expenseSubCategory == null)
            {
                return NotFound();
            }
            return View(expenseSubCategory);
        }

        // POST: ExpenseSubCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ExpenseSubCategory expenseSubCategory)
        {
            if (id != expenseSubCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseSubCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseSubCategoryExists(expenseSubCategory.Id))
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
            return View(expenseSubCategory);
        }

        // GET: ExpenseSubCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseSubCategory = await _context.ExpenseSubCategorys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseSubCategory == null)
            {
                return NotFound();
            }

            return View(expenseSubCategory);
        }

        // POST: ExpenseSubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var expenseSubCategory = await _context.ExpenseSubCategorys.FindAsync(id);
            if (expenseSubCategory != null)
            {
                _context.ExpenseSubCategorys.Remove(expenseSubCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseSubCategoryExists(Guid id)
        {
            return _context.ExpenseSubCategorys.Any(e => e.Id == id);
        }
    }
}
