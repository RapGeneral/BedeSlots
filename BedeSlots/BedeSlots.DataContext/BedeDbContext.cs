using BedeSlots.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BedeSlots.DataContext
{
    public class BedeDBContext : IdentityDbContext<User>
    {
        public DbSet<Balance> Balances { get; set; }

        public DbSet<BankDetails> BankDetails { get; set; }

        public DbSet<Currency> Currencies { get; set; }
         
        public DbSet<Rate> Rates { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public DbSet<UserBankDetails> UserBankDetails { get; set; }

        public BedeDBContext(DbContextOptions<BedeDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBankDetails>()
                .HasKey(e => new { e.UserId, e.BankDetailsId });

            builder.Entity<Rate>()
                .HasOne(r => r.ToCurrency)
                .WithMany(tc => tc.Rates)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.Balance)
                .WithMany(b => b.Transactions)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

    }
}
