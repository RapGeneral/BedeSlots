using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.DataContext.Configurations
{
    internal class UserBankDetailsConfiguration : IEntityTypeConfiguration<UserBankDetails>
    {
        public void Configure(EntityTypeBuilder<UserBankDetails> builder)
        {
            builder.HasKey(ubd => new { ubd.UserId, ubd.BankDetailsId });

            builder.HasOne(ubd => ubd.BankDetails)
                .WithMany(bd => bd.UserBankDetails)
                .IsRequired(true);

            builder.HasOne(ubd => ubd.User)
                .WithMany(bd => bd.UserBankDetails)
                .IsRequired(true);

            builder.Property(ubd => ubd.IsDeleted);
            builder.Property(ubd => ubd.DeletedOn);
        }
    }
}