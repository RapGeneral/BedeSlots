using BedeSlots.DataContext.Configurations;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public DbSet<BalanceType> BalanceTypes { get; set; }

        public BedeDBContext(DbContextOptions<BedeDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BalanceConfigurations());
            builder.ApplyConfiguration(new BalanceTypeConfigurations());
            builder.ApplyConfiguration(new BankDetailsConfigurations());
            builder.ApplyConfiguration(new CurrencyConfigurations());
            builder.ApplyConfiguration(new RateConfigurations());
            builder.ApplyConfiguration(new TransactionConfigurations());
            builder.ApplyConfiguration(new TransactionTypeConfiguration());
            builder.ApplyConfiguration(new UserBankDetailsConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
