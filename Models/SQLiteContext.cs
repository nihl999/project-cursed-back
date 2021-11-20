using Microsoft.EntityFrameworkCore;

namespace project_cursed.Models
{
    public class SQLiteContext : DbContext
    {
        public SQLiteContext() { }
        public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Question>().HasKey(m => m.Id);
            builder.Entity<Question>().HasMany(m => m.Answers).WithOne(m => m.Question);
            base.OnModelCreating(builder);
        }
    }
}


