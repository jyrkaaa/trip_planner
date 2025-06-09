using Base.Contracts;

namespace App.DTO.v1;

public class ExpenseCategory : IDomainId
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public ICollection<ExpenseSubCategory>? ExpenseSubCategories { get; set; }
}