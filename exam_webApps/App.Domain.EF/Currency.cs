using Base.Domain;

namespace App.Domain.EF;

public class Currency : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
    
    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<Trip>? Trips { get; set; }
    
}