using Base.Contracts;
using Base.Domain.Identity;

namespace App.Domain.EF.Identity;

public class AppRefreshToken : BaseRefreshToken, IDomainUserId
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}