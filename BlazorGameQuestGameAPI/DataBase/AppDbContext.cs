
using Microsoft.EntityFrameworkCore;
using BlazorGameQuestClassLib;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Player> Players{ get; set; }
    }
}