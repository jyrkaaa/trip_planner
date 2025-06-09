using Base.Contracts;

namespace App.DAL.DTO;

public class ExpenseCategory : IDomainId
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = default!;

    public ICollection<ExpenseSubCategory>? ExpenseSubCategories { get; set; }

    public ICollection<Expense>? Expenses { get; set; }
}