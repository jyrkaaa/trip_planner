using System.ComponentModel.DataAnnotations;
using Base.Contracts;
using Base.Domain;

namespace App.DAL.DTO;

public class AppUser : IDomainId
{
    
    public string? Username { get; set; }

    public string? Email { get; set; }
    public Guid Id { get; set; }
}