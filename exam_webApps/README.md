ef
~~~sh 
dotnet ef migrations add --project App.DAL --startup-project WebApp --context AppDbContext InitialCreate

dotnet ef migrations   --project App.DAL --startup-project WebApp remove

dotnet ef database   --project App.DAL --startup-project WebApp update
dotnet ef database   --project App.DAL --startup-project WebApp drop

~~~
MVC Controllers
~~~sh
cd WebApp

dotnet aspnet-codegenerator controller -name TripController        -actions -m  App.Domain.EF.Trip        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name DestinationController        -actions -m  App.Domain.EF.Destination        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name DestinationInTripController        -actions -m  App.Domain.EF.DestinationInTrip        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CurrencyController        -actions -m  App.Domain.EF.Currency        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExpenseController        -actions -m  App.Domain.EF.Expense        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExpenseCategoryController        -actions -m  App.Domain.EF.ExpenseCategory        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExpenseSubCategoryController        -actions -m  App.Domain.EF.ExpenseSubCategory        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f


dotnet aspnet-codegenerator identity -dc App.DAL.AppDbContext -f
~~~
API Controllers
~~~sh
dotnet aspnet-codegenerator controller -name TripController  -m  App.Domain.EF.Trip        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ExpenseSubCategoryController  -m  App.Domain.EF.ExpenseSubCategory        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ExpenseCategoryController  -m  App.Domain.EF.ExpenseCategory        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ExpenseController  -m  App.Domain.EF.Expense        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name CurrencyController  -m  App.Domain.EF.Currency        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name DestinationController  -m  App.Domain.EF.Destination        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name DestinationInTripController  -m  App.Domain.EF.DestinationInTrip        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f


For ADMin: mvc

cd WebApp

dotnet aspnet-codegenerator controller -name ExerciseController        -actions -m  App.Domain.EF.Exercise        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UserWeightController        -actions -m  App.Domain.EF.UserWeight        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name WorkoutController        -actions -m  App.Domain.EF.Workout        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExerInWorkoutController        -actions -m  App.Domain.EF.ExerInWorkout        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExerciseCategoryController        -actions -m  App.Domain.EF.ExerciseCategory        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExerTargerController        -actions -m  App.Domain.EF.ExerTarget        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name SetInExercController        -actions -m  App.Domain.EF.SetInExerc        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UsersInWorkoutController        -actions -m  App.Domain.EF.UsersInWorkout        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExerGuideController        -actions -m  App.Domain.EF.ExerGuide        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

~~~
~~~sh
In Backend catalog
docker run --name gym_docker_be --rm -it -p 8888:8080 webapp
~~~