using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesApi.src.Models;

namespace SalesApi.src.Data;

public class SalesContext: IdentityDbContext{
    public SalesContext(DbContextOptions<SalesContext> opts)
        : base(opts){

    }

     public DbSet<Debt> Debts { get; set; }
     public DbSet<Customer> Customers { get; set; }

     public DbSet<User> User { get; set; }
}
