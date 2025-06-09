using Base.Domain;

namespace App.Domain.EF;

public class ExpenseCategory : BaseEntity
{
    public string CategoryName { get; set; } = string.Empty;

    public ICollection<ExpenseSubCategory>? ExpenseSubCategories { get; set; }
}