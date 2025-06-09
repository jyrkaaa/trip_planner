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

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpenseSubCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseSubCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExpenseSubCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.ExpenseSubCategory>>> GetExpenseSubCategorys()
        {
            var list = await _context.ExpenseSubCategorys.ToListAsync();
            return list.Select(s => new App.DTO.v1.ExpenseSubCategory
            {
                Id = s.Id,
                Name = s.Name,
                ExpenseCategoryId = s.ExpenseCategoryId,
            }).ToList();
        }

        // GET: api/ExpenseSubCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.ExpenseSubCategory>> GetExpenseSubCategory(Guid id)
        {
            var expenseSubCategory = await _context.ExpenseSubCategorys.FindAsync(id);

            if (expenseSubCategory == null)
            {
                return NotFound();
            }

            return new App.DTO.v1.ExpenseSubCategory { Id = expenseSubCategory.Id, Name = expenseSubCategory.Name };
        }

        // PUT: api/ExpenseSubCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseSubCategory(Guid id, ExpenseSubCategory expenseSubCategory)
        {
            if (id != expenseSubCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenseSubCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseSubCategoryExists(id))
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

        // POST: api/ExpenseSubCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.ExpenseSubCategory>> PostExpenseSubCategory(ExpenseSubCategoryCreate expenseSubCategory)
        {
            var efEntity = new App.Domain.EF.ExpenseSubCategory
            {
                Id = Guid.NewGuid(), ExpenseCategoryId = expenseSubCategory.ExpenseCategoryId,
                Name = expenseSubCategory.Name
            };
            _context.ExpenseSubCategorys.Add(efEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenseSubCategory", new { id = efEntity.Id }, expenseSubCategory);
        }

        // DELETE: api/ExpenseSubCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseSubCategory(Guid id)
        {
            var expenseSubCategory = await _context.ExpenseSubCategorys.FindAsync(id);
            if (expenseSubCategory == null)
            {
                return NotFound();
            }

            _context.ExpenseSubCategorys.Remove(expenseSubCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseSubCategoryExists(Guid id)
        {
            return _context.ExpenseSubCategorys.Any(e => e.Id == id);
        }
    }
}
