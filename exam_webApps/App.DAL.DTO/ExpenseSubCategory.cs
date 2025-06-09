using Base.Contracts;

namespace App.DAL.DTO;

public class ExpenseSubCategory : IDomainId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public Guid ExpenseCategoryId { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; } = default!;
}