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
using Expense = App.Domain.EF.Expense;
using ExpenseCategory = App.DTO.v1.ExpenseCategory;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Expense
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.Expense>>> GetExpenses()
        {
            
            var list = await _context.Expenses.
                Include(e => e.ExpenseSubCategory)
                    .ThenInclude(c => c!.ExpenseCategory)
                .Include(e => e.Trip)
                .Where(e => e.Trip!.UserId == User.GetUserId())
                .ToListAsync();
            return list.Select(c => new App.DTO.v1.Expense()
            {
                Id = c.Id,
                ExpenseSubCategoryId = c.ExpenseSubCategoryId,
                OriginalAmount = c.OriginalAmount,
                BaseAmount = c.BaseAmount,
                Description = c.Description,
                TripId = c.TripId,
                
                ExpenseSubCategory = new ExpenseSubCategory()
                {
                    Id = c.ExpenseSubCategoryId!.Value,
                    Name = c.ExpenseSubCategory!.Name,
                    ExpenseCategory = new App.DTO.v1.ExpenseCategory
                    {
                        Id = c.ExpenseSubCategory!.ExpenseCategoryId,
                        CategoryName = c.ExpenseSubCategory!.ExpenseCategory.CategoryName,
                    }
                },
            }).ToList();
        }

        // GET: api/Expense/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.Expense>> GetExpense(Guid id)
        {
            var c = await _context.Expenses.
                Include(e => e.ExpenseSubCategory)
                    .ThenInclude(c => c!.ExpenseCategory)
                .Include(e => e.Trip)
                .Where(e => e.Trip!.UserId == User.GetUserId())
                .FirstAsync(e => e.Id == id);

            if (c == null)
            {
                return NotFound();
            }

            return new App.DTO.v1.Expense()
            {
                Id = c.Id,
                ExpenseSubCategoryId = c.ExpenseSubCategoryId,
                OriginalAmount = c.OriginalAmount,
                BaseAmount = c.BaseAmount,
                Description = c.Description,
                TripId = c.TripId,
                
                ExpenseSubCategory = new ExpenseSubCategory()
                {
                    Id = c.ExpenseSubCategoryId!.Value,
                    Name = c.ExpenseSubCategory!.Name,
                    ExpenseCategory = new App.DTO.v1.ExpenseCategory
                    {
                        Id = c.ExpenseSubCategory!.ExpenseCategoryId,
                        CategoryName = c.ExpenseSubCategory!.ExpenseCategory.CategoryName,
                    }
                    
                }
            };
        }

        // PUT: api/Expense/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(Guid id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
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

        // POST: api/Expense
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(ExpenseCreate expense)
        {
            var currencyEntity = await _context.Currencies.FindAsync(expense.CurrencyId);
            var subCatEntity = await _context.ExpenseSubCategorys.FindAsync(expense.ExpenseSubCategoryId);
            var efEntity = new Expense()
            {
                Id = Guid.NewGuid(),
                ExpenseSubCategoryId = subCatEntity!.Id,
                OriginalAmount = expense.AmountOriginal,
                BaseAmount = expense.AmountOriginal * currencyEntity!.Rate,
                Description = expense.Description,
                TripId = expense.TripId,
                CurrencyId = expense.CurrencyId,


            };
            _context.Expenses.Add(efEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = efEntity.Id }, expense);
        }

        // DELETE: api/Expense/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(Guid id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
