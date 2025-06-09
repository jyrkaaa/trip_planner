using Base.DAL.EF;
using App.DAL.Contracts;

namespace App.DAL;

public class AppUOW : BaseUOW<AppDbContext> , IAppUOW
    
{
    public AppUOW(AppDbContext uowDbContext) : base(uowDbContext)
    {
    }

    }