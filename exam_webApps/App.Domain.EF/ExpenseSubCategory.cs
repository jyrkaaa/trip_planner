using Base.Domain;

namespace App.Domain.EF;

public class ExpenseSubCategory : BaseEntity
{
    public string Name { get; set; } = default!;

    public Guid ExpenseCategoryId { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; } = default!;
    public ICollection<Expense>? Expenses { get; set; } = default!;
}