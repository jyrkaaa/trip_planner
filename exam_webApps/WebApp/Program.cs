using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using App.DAL;
using App.DAL.Contracts;
using App.DAL.DataSeeding;
using App.Domain.EF.Identity;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Base.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;
using WebApp.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//connectionString = connectionString.Replace("<%location%>", FileHelper.BasePath);
Console.WriteLine(builder.Environment.IsProduction());
if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
            connectionString,
            b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString,
                b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    
}


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IAppUOW, AppUOW>();

builder.Services.AddIdentity<AppUser, AppRole>(o =>
        o.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultUI();


// remove default claim mapping
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services
    .AddAuthentication()
    .AddCookie(options => { options.SlidingExpiration = true; })
    .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            //options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = builder.Configuration["JWTSecurity:Issuer"],
                ValidAudience = builder.Configuration["JWTSecurity:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JWTSecurity:Key"]!)),
                ClockSkew = TimeSpan.Zero
            };
        }
    );



builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserNameResolver, UserNameResolver>();



// Add culture support
/*var supportedCultures = builder.Configuration
    .GetSection("SupportedCultures")
    .GetChildren()
    .Select(x => new CultureInfo(x.Value!))
    .ToArray();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // datetime and currency support
    options.SupportedCultures = supportedCultures;
    // UI translated strings
    options.SupportedUICultures = supportedCultures;
    // if nothing is found, use this
    options.DefaultRequestCulture =
        new RequestCulture(
            builder.Configuration["DefaultCulture"]!, builder.Configuration["DefaultCulture"]!);
    options.SetDefaultCulture(builder.Configuration["DefaultCulture"]!);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // Order is important, its in which order they will be evaluated
        // add support for ?culture=ru-RU
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.SetIsOriginAllowed((host) => true);
    });
});

var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    // in case of no explicit version
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();


// ========================================================================
var app = builder.Build();
// ========================================================================
SetupAppData(app, app.Environment, app.Configuration);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRequestLocalization(options: app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value);

app.UseRouting();

app.UseCors("CorsAllowAll");

app.UseSwagger();
app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
        }
        // serve from root
        // options.RoutePrefix = string.Empty;
    }
);

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name : "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();
    
app.Run();

static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
{
    using var serviceScope = ((IApplicationBuilder)app).ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<IApplicationBuilder>>();

    using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

    WaitDbConnection(context, logger);

    using var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    using var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();


    if (context.Database.ProviderName!.Contains("InMemory"))
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return;
    }

    if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
    {
        logger.LogWarning("DropDatabase");
        AppDataInit.DeleteDatabase(context);
    }

    if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
    {
        logger.LogInformation("MigrateDatabase");
        AppDataInit.MigrateDatabase(context);
    }

    if (configuration.GetValue<bool>("DataInitialization:SeedIdentity"))
    {
        logger.LogInformation("SeedIdentity");
        AppDataInit.SeedIdentity(userManager, roleManager);
        AppDataInit.SeedCountries(context);
        AppDataInit.SeedCurrencys(context);
        AppDataInit.SeedCategories(context);
        AppDataInit.SeedSubCategories(context);
        
        //For preset Exercise Categorys
    }

    if (configuration.GetValue<bool>("DataInitialization:SeedData"))
    {
        logger.LogInformation("SeedData");
        AppDataInit.SeedAppData(context);
    }
}

static void WaitDbConnection(AppDbContext ctx, ILogger<IApplicationBuilder> logger)
{
    // TODO: Login failed for user 'sa'. Reason: Failed to open the explicitly specified database 'nutikas'. [CLIENT: 172.18.0.3]
    // could actually log in, but db was not there - migrations where not applied yet

    // maybe Database.OpenConnection

    while (true)
    {
        try
        {
            ctx.Database.OpenConnection();
            ctx.Database.CloseConnection();
            return;
        }
        catch (Npgsql.PostgresException e)
        {
            logger.LogWarning("Checked postgres db connection. Got: {}", e.Message);

            if (e.Message.Contains("does not exist"))
            {
                logger.LogWarning("Applying migration, probably db is not there (but server is)");
                return;
            }

            logger.LogWarning("Waiting for db connection. Sleep 1 sec");
            System.Threading.Thread.Sleep(1000);
        }
    }
}


// ======================================================================================================
// needed for unit testing, to change generated top level statement class to public
public partial class Program
{
}
