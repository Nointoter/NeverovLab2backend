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
        public DbSet<Member> Members{ get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Tale> Tales { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public pgDbContext()
        {
            _pgsqlConnectionString =
                $"Host=localhost;"+
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
