namespace App.DAL.DataSeeding;

public static class InitialData
{
    public static readonly (string roleName, Guid? id)[]
        Roles =
        [
            ("admin", null),
        ];

    public static readonly (string name, string password, Guid? id, string[] roles)[]
        Users =
        [
            ("admin@taltech.ee", "Foo.Bar.1", null, ["admin"]),
            ("jurgen@gmail.com", "StrongPa1!ssword", null, []),
        ];

    public static readonly (Guid id, string code, string Name, decimal Rate, string createdBy, DateTimeOffset CreatedAt,
        string? ChangedBy, DateTimeOffset? ChangedAt, string? SysNotes )[]
        Currencies =
        [
            (Guid.NewGuid(), "EUR", "Euro", 1.00m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "USD", "US Dollar", 0.92m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "GBP", "British Pound", 1.15m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "JPY", "Japanese Yen", 0.0062m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "CHF", "Swiss Franc", 1.02m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "CAD", "Canadian Dollar", 0.68m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "AUD", "Australian Dollar", 0.61m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "SEK", "Swedish Krona", 0.085m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "NOK", "Norwegian Krone", 0.087m, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "PLN", "Polish Zloty", 0.23m, "system", DateTimeOffset.UtcNow, null, null, null)

        ];

    public static readonly (Guid id, string CountryName, string createdBy, DateTimeOffset CreatedAt,
        string? ChangedBy, DateTimeOffset? ChangedAt, string? SysNotes )[]
        Countries =
        [
            (Guid.NewGuid(), "Estonia", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Germany", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "France", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Spain", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Italy", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "United States", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Canada", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Sweden", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Poland", "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Norway", "system", DateTimeOffset.UtcNow, null, null, null)
        ];

    public static readonly Guid FoodCategoryId = Guid.NewGuid();
    public static readonly Guid EntertainmentCategoryId = Guid.NewGuid();
    public static readonly (Guid id, string CategoryName, string createdBy, DateTimeOffset CreatedAt,
        string? ChangedBy, DateTimeOffset? ChangedAt, string? SysNotes )[]
        Categorys =
        [
            (FoodCategoryId, "Food", "system", DateTimeOffset.UtcNow, null, null, null),
            (EntertainmentCategoryId, "Entertainment", "system", DateTimeOffset.UtcNow, null, null, null),
        ];
    public static readonly (Guid id, string Name, Guid categoryId, string createdBy, DateTimeOffset CreatedAt,
        string? ChangedBy, DateTimeOffset? ChangedAt, string? SysNotes )[]
        SubCategorys =
        [
            (Guid.NewGuid(), "Eating Out", FoodCategoryId, "system", DateTimeOffset.UtcNow, null, null, null),
            (Guid.NewGuid(), "Theater", EntertainmentCategoryId,  "system", DateTimeOffset.UtcNow, null, null, null),
        ];
}