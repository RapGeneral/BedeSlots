using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.DataContext.Configurations
{
    internal class TransactionConfigurations : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Type);

            builder.HasOne(t => t.Balance)
               .WithMany(b => b.Transactions)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Date)
                .IsRequired(true);
            builder.Property(t => t.Description)
                .IsRequired(true);
            builder.Property(t => t.Date)
                .IsRequired(true);

            builder.Property(t => t.OpeningBalance);
            builder.Property(t => t.Amount);
        }
    }
}
