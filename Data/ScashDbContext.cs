using Microsoft.EntityFrameworkCore;
using Scash.Models;

namespace Scash.Data
{
    public class ScashDbContext : DbContext
    {
        public ScashDbContext(DbContextOptions<ScashDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<SystemSettings> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.PhoneNumber).IsUnique();
                entity.Property(u => u.Balance).HasPrecision(18, 2);
                entity.Property(u => u.DailyTransactionLimit).HasPrecision(18, 2);
                entity.Property(u => u.MonthlyTransactionLimit).HasPrecision(18, 2);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(t => t.ReferenceNumber).IsUnique();
                entity.Property(t => t.Amount).HasPrecision(18, 2);
                entity.Property(t => t.Fee).HasPrecision(18, 2);
                entity.Property(t => t.Commission).HasPrecision(18, 2);
                entity.HasOne(t => t.Sender)
                    .WithMany()
                    .HasForeignKey(t => t.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(t => t.Recipient)
                    .WithMany()
                    .HasForeignKey(t => t.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TransactionHistory configuration
            modelBuilder.Entity<TransactionHistory>(entity =>
            {
                entity.Property(t => t.Amount).HasPrecision(18, 2);
                entity.Property(t => t.Balance).HasPrecision(18, 2);
                entity.Property(t => t.Commission).HasPrecision(18, 2);
                entity.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SystemSettings configuration
            modelBuilder.Entity<SystemSettings>(entity =>
            {
                entity.HasIndex(s => s.Key).IsUnique();
            });
        }
    }
} 