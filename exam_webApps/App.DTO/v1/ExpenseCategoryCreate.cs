using Base.Contracts;

namespace App.DTO.v1;

public class ExpenseCategoryCreate
{
    public string CategoryName { get; set; } = default!;
}