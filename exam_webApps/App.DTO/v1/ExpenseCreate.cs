using Base.Contracts;

namespace App.DTO.v1;

public class ExpenseCreate
{
    public decimal AmountOriginal { get; set; }
    public string? Description { get; set; }

    public Guid? ExpenseSubCategoryId { get; set; }
    public Guid CurrencyId { get; set; } = default!;
    public Guid TripId { get; set; } = default!;
}