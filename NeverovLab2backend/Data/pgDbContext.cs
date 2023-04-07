using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Models;

namespace NeverovLab2backend.Data
{

    public class pgDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public pgDbContext(DbContextOptions options) : base(options)
        {
        }
        private readonly string _pgsqlConnectionString;
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<Tale> Tales => Set<Tale>();
        public DbSet<Session> Sessions => Set<Session>();

        public pgDbContext()
        {
            _pgsqlConnectionString =
                $"Server=PostgreSQL15;" +
                $"Port=5432;"+
                $"Database=D&Ddb;"+
                $"Username=postgres;"+
                $"Password=postgres";
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_pgsqlConnectionString);

        }
    }
}
