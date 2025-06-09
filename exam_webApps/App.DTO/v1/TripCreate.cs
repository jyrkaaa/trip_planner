using Base.Contracts;

namespace App.DTO.v1;

public class TripCreate
{
    public decimal BudgetOriginal { get; set; }
    public DateTime StartDate { get; set; }
    public string Name { get; set; } = default!;
    public DateTime EndDate { get; set; }
    public bool Public { get; set; }

    public Guid CurrencyId { get; set; } = default!;
    public Guid DestinationId { get; set; } = default!;
    
}