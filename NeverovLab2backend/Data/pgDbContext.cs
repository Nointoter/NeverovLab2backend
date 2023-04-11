using Microsoft.EntityFrameworkCore;

namespace NeverovLab2backend.Data;

public class pgDbContext : DbContext
{
    public pgDbContext(DbContextOptions<pgDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }
    public DbSet<User> Users
    {
        get;
        set;
    }
    public DbSet<Member> Members
    {
        get;
        set;
    }
    public DbSet<Character> Characters
    {
        get;
        set;
    }
    public DbSet<Tale> Tales
    {
        get;
        set;
    }
    public DbSet<Session> Sessions
    {
        get;
        set;
    }
}