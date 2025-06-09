using Base.Contracts;

namespace App.DAL.DTO;

public class Currency : IDomainId
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
    
    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<Trip>? Trips { get; set; }
    
}