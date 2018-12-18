using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.DataContext.Configurations
{
    internal class RateConfigurations : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.ToCurrency)
                .WithMany(c => c.Rates)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.Coeff)
                .IsRequired(true);

            builder.Property(bd => bd.CreatedAt);
        }
    }
}
