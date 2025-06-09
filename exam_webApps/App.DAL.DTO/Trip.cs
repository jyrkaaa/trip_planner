using Base.Contracts;

namespace App.DAL.DTO;

public class Trip : IDomainId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public decimal? BudgetBase { get; set; }
    public decimal BudgetOriginal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Public { get; set; }

    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }
    
    public string CurrencyCode { get; set; } = default!;
    public Currency Currency { get; set; } = default!;

    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<DestinationInTrip>? DestinationsInTrip { get; set; }
}