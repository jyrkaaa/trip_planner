using Base.Contracts;

namespace App.DTO.v1;

public class Expense : IDomainId
{
    public Guid Id { get; set; }
    public decimal BaseAmount { get; set; }
    public decimal OriginalAmount { get; set; }
    public string? Description { get; set; }

    public Guid? ExpenseSubCategoryId { get; set; }
    public ExpenseSubCategory? ExpenseSubCategory { get; set; }
    public Guid CurrencyId { get; set; } = default!;
    public Currency? Currency { get; set; } = default!;
    public Guid TripId { get; set; } = default!;
    public Trip? Trip { get; set; } = default!;
}