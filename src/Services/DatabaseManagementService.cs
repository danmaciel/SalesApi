
using Microsoft.EntityFrameworkCore;
using SalesApi.src.Data;

namespace SalesApi.src.Services;

public static class DatabaseManagementService{

    public static void MigrationInitialisation(IApplicationBuilder app){
        using(var serviceScope = app.ApplicationServices.CreateScope()){
            var serviceDb = serviceScope.ServiceProvider.GetService<SalesContext>();
            serviceDb?.Database.Migrate();
        }
    }
 
}