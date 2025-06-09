using App.Domain.EF.Identity;
using Base.Domain;

namespace App.Domain.EF;

public class Trip : BaseEntity
{
    public decimal BudgetOriginal { get; set; }
    public decimal? BudgetBase { get; set; }
    public DateTime StartDate { get; set; }
    public string Name { get; set; } = default!;

    public DateTime EndDate { get; set; }
    public bool Public { get; set; }

    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid CurrencyId { get; set; } = default!;
    public Currency Currency { get; set; } = default!;

    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<DestinationInTrip>? DestinationsInTrip { get; set; }
}