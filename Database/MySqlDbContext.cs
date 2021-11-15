using Microsoft.EntityFrameworkCore;
using NetFileAPI.Models;

namespace NetFileAPI.Database
{
    public class MySqlDbContext : DbContext
    {
        public DbSet<FileModel> FileModels { get; set; } = null!;

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileModel>()
                .Property(c => c.StorageType)
                .HasConversion<int>();
        }
    }
}