using DatingApp.API.Modles;
using Microsoft.EntityFrameworkCore;

// This ia where you tell your database Context about the Data Models you want to affect from the "Model" folder
namespace DatingApp.API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}

        public DbSet<Values> Values { get; set; }
        public DbSet<User> Users { get; set; } 
        
    }
}