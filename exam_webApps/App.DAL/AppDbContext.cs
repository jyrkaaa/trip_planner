using App.Domain;
using App.Domain.EF;
using App.Domain.EF.Identity;
using Base.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.DAL;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>

{
    public DbSet<Trip> Trips { get; set; } = default!;
    public DbSet<Expense> Expenses { get; set; } = default!;
    public DbSet<ExpenseCategory> ExpenseCategorys { get; set; } = default!;
    public DbSet<ExpenseSubCategory> ExpenseSubCategorys { get; set; } = default!;
    public DbSet<Destination> Destinations { get; set; } = default!;
    public DbSet<DestinationInTrip> DestinationInTrips { get; set; } = default!;
    public DbSet<Currency> Currencies { get; set; } = default!;
    
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    private readonly IUserNameResolver _userNameResolver;
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(DbContextOptions<AppDbContext> options, IUserNameResolver usernameResolver, ILogger<AppDbContext> logger)
        : base(options)
    {
        _userNameResolver = usernameResolver;
        _logger = logger;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var addedEntries = ChangeTracker.Entries()
            ;
        foreach (var entry in addedEntries)
        {
            if (entry is { Entity: IDomainMeta })
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        (entry.Entity as IDomainMeta)!.CreatedAt = DateTime.UtcNow;
                        (entry.Entity as IDomainMeta)!.CreatedBy = _userNameResolver.CurrentUserId ?? "system";
                        break;
                    case EntityState.Modified:
                        entry.Property("ChangedAt").IsModified = true;
                        entry.Property("ChangedBy").IsModified = true;
                        (entry.Entity as IDomainMeta)!.ChangedAt = DateTime.UtcNow;
                        (entry.Entity as IDomainMeta)!.ChangedBy = _userNameResolver.CurrentUserId;

                        // Prevent overwriting CreatedBy/CreatedAt on update
                        entry.Property("CreatedAt").IsModified = false;
                        entry.Property("CreatedBy").IsModified = false;
                        break;
                }
            }

        }


        return base.SaveChangesAsync(cancellationToken);

    }
    public Task<int> SaveChangesAsyncWithhoutHardcode(CancellationToken cancellationToken = new CancellationToken())
    {
        var addedEntries = ChangeTracker.Entries()
            ;
        foreach (var entry in addedEntries)
        {
            if (entry is { Entity: IDomainMeta })
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        (entry.Entity as IDomainMeta)!.CreatedAt = DateTime.UtcNow;
                        (entry.Entity as IDomainMeta)!.CreatedBy = _userNameResolver.CurrentUserId ?? "system";
                        break;
                    case EntityState.Modified:
                        entry.Property("ChangedAt").IsModified = true;
                        entry.Property("ChangedBy").IsModified = true;
                        (entry.Entity as IDomainMeta)!.ChangedAt = DateTime.UtcNow;
                        (entry.Entity as IDomainMeta)!.ChangedBy = _userNameResolver.CurrentUserId;
                        break;
                }
            }

        }


        return base.SaveChangesAsync(cancellationToken);

    }

    
    
}