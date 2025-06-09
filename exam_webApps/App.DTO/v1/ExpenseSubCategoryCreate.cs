using Base.Contracts;

namespace App.DTO.v1;

public class ExpenseSubCategoryCreate
{
    public string Name { get; set; } = default!;
    public Guid ExpenseCategoryId { get; set; }
}