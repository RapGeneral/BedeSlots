using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.DataContext.Configurations
{
    internal class BalanceConfigurations : IEntityTypeConfiguration<Balance>
    {
        public void Configure(EntityTypeBuilder<Balance> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.User)
                .WithMany(u => u.Balances)
                .IsRequired(true);

            builder.HasOne(b => b.Type);

            builder.HasOne(b => b.Currency);

            builder.HasMany(b => b.Transactions)
                .WithOne(t => t.Balance);

            builder.Property(b => b.Money);
        }
    }
}
