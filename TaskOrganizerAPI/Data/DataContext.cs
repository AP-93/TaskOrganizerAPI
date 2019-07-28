using Microsoft.EntityFrameworkCore;
using TaskOrganizerAPI.Model;

namespace TaskOrganizerAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){ }
        public DbSet<User> Users { get; set; }
    }
}
