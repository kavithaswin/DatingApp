using Microsoft.EntityFrameworkCore;
using DatingAPI.Entities;
namespace DatingAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users{get;set;}
    }
}