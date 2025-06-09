using Base.Contracts;

namespace App.DTO.v1;

public class Currency : IDomainId
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }
    
    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<Trip>? Trips { get; set; }
    
}