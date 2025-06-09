using Base.Contracts;

namespace App.DTO.v1;

public class ExpenseSubCategory : IDomainId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid ExpenseCategoryId { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; } = default!;
    public ICollection<Expense>? Expenses { get; set; } = default!;
}