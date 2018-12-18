using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.DataContext.Configurations
{
    internal class CurrencyConfigurations : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CurrencyName)
                .IsRequired(true);
        }
    }
}