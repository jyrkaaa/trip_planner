using System;
using System.Collections;
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
using ExpenseSubCategory = App.DTO.v1.ExpenseSubCategory;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ExpenseCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExpenseCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.ExpenseCategory>>> GetExpenseCategorys()
        {
            var list = await _context.ExpenseCategorys.Include(e => e.ExpenseSubCategories).ToListAsync();
            var result = list.Select(c => new App.DTO.v1.ExpenseCategory
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                ExpenseSubCategories = c.ExpenseSubCategories?.Select(c => new App.DTO.v1.ExpenseSubCategory
                {
                    Id = c.Id,
                    ExpenseCategoryId = c.ExpenseCategoryId,
                    Name = c.Name,
                }).ToList()
            }).ToList();
            return Ok(result);
        }

        // GET: api/ExpenseCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.ExpenseCategory>> GetExpenseCategory(Guid id)
        {
            var expenseCategory = await _context.ExpenseCategorys.Include(e => e.ExpenseSubCategories).FirstOrDefaultAsync(e => e.Id == id);

            if (expenseCategory == null)
            {
                return NotFound();
            }

            return new ExpenseCategory { 
                Id = expenseCategory.Id,
                CategoryName = expenseCategory.CategoryName,
                ExpenseSubCategories = expenseCategory.ExpenseSubCategories?.Select(c => new App.DTO.v1.ExpenseSubCategory
                {
                    Id = c.Id,
                    ExpenseCategoryId = c.ExpenseCategoryId,
                    Name = c.Name,
                }).ToList()
            };
        }

        // PUT: api/ExpenseCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseCategory(Guid id, App.DTO.v1.ExpenseCategory expenseCategory)
        {
            if (id != expenseCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenseCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseCategoryExists(id))
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

        // POST: api/ExpenseCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.ExpenseCategory>> PostExpenseCategory(App.DTO.v1.ExpenseCategoryCreate expenseCategory)
        {
            var efEntity = new App.Domain.EF.ExpenseCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = expenseCategory.CategoryName,
            };
            _context.ExpenseCategorys.Add(efEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenseCategory", new { id = efEntity.Id }, expenseCategory);
        }

        // DELETE: api/ExpenseCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseCategory(Guid id)
        {
            var expenseCategory = await _context.ExpenseCategorys.FindAsync(id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            _context.ExpenseCategorys.Remove(expenseCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseCategoryExists(Guid id)
        {
            return _context.ExpenseCategorys.Any(e => e.Id == id);
        }
    }
}
