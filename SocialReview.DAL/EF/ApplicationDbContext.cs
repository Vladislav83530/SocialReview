using Microsoft.EntityFrameworkCore;
using SocialReview.DAL.Entities;

namespace SocialReview.DAL.EF
{
    /// <summary>
    /// Application database context
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Establishment> Establishments { get; set; }
        public DbSet<EstablishmentPhoto> EstablishmentPhotos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<EaterieReview> EaterieReviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        { 
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
                .HasMany<Review>()
                .WithOne(e => e.User)
                .HasForeignKey(k => k.CustomerId);

            builder.Entity<Establishment>()
                .HasMany<Review>()
                .WithOne(e => e.Establishment)
                .HasForeignKey(c => c.EstablishmentId);

            builder.Entity<Review>()
                .HasOne<EaterieReview>()
                .WithOne(c => c.Review)
                .HasForeignKey<EaterieReview>(c => c.ReviewId);

            builder.Entity<Establishment>()
                .HasMany<EstablishmentPhoto>()
                .WithOne(e => e.Establishment)
                .HasForeignKey(c => c.EstablishmentId);

            builder.Entity<Customer>()
                .HasOne<User>()
                .WithOne(c => c.Customer)
                .HasForeignKey<User>(c => c.CustomerId);

            builder.Entity<Establishment>()
                .HasOne<User>()
                .WithOne(c => c.Establishment)
                .HasForeignKey<User>(c => c.EstablishmentId);

        }
    }
}
