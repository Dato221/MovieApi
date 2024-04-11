using Microsoft.EntityFrameworkCore;
using SimplifiedIMDBApi.Entities;

namespace SimplifiedIMDBApi.DataAccess
{
    public class SimpleMoviesDbContext : DbContext
    {
        public SimpleMoviesDbContext(DbContextOptions<SimpleMoviesDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-SPTV65J\\SQLEXPRESS; Database=NewMovieDatabase; Trusted_Connection=True; TrustServerCertificate=true");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Actor> Actors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movies>()
        .HasKey(m => m.MovieId);

            modelBuilder.Entity<MovieUser>()
                .HasKey(um => new { um.UserId, um.MovieId });

            modelBuilder.Entity<MovieUser>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMovies)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<MovieUser>()
                .HasOne(um => um.Movie)
                .WithMany(m => m.UserMovies)
                .HasForeignKey(um => um.MovieId);
            modelBuilder.Entity<AdminUser>()
                .HasKey(au => new { au.AdminId, au.UserId });

            modelBuilder.Entity<AdminUser>()
                .HasOne(au => au.Admin)
                .WithMany(a => a.AdminUsers)
                .HasForeignKey(au => au.AdminId);

            modelBuilder.Entity<AdminUser>()
                .HasOne(au => au.Users)
                .WithMany(u => u.AdminUsers)
                .HasForeignKey(au => au.UserId);

            modelBuilder.Entity<ActorMovies>()
                .HasOne(ac=>ac.Actor)
                .WithMany(n=>n.ActorMovies)
                .HasForeignKey(ac => ac.ActorId);
            modelBuilder.Entity<ActorMovies>()
                .HasOne(ac => ac.Movies)
                .WithMany(n => n.ActorMovies)
                .HasForeignKey(ac => ac.MovieId);
        }
    }
}
