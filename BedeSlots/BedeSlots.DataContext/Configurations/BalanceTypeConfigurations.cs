using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.DataContext.Configurations
{
    internal class BalanceTypeConfigurations : IEntityTypeConfiguration<BalanceType>
    {
        public void Configure(EntityTypeBuilder<BalanceType> builder)
        {
            builder.HasKey(bt => bt.Id);

            builder.Property(bt => bt.Name)
                .HasMaxLength(20)
                .IsRequired(true);
        }
    }
}
