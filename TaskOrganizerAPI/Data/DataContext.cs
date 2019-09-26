using Microsoft.EntityFrameworkCore;
using TaskOrganizerAPI.Model;

namespace TaskOrganizerAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){

       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBoard>().HasKey(sc => new { sc.UserId, sc.BoardId });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<UserBoard> UserBoards { get; set; }
    }
}
