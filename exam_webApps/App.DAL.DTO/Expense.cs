using Base.Contracts;
using Base.Domain;

namespace App.DAL.DTO;

public class Expense : IDomainId
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public Guid? ExpenseCategoryId { get; set; }
    public ExpenseCategory? ExpenseCategory { get; set; }

    public Guid TripId { get; set; } = default!;
    public Trip Trip { get; set; } = default!;
}