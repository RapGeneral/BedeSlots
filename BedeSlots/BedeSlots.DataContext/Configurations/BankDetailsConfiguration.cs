using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataContext.Configurations
{
    internal class BankDetailsConfigurations : IEntityTypeConfiguration<BankDetails>
    {
        public void Configure(EntityTypeBuilder<BankDetails> builder)
        {
            builder.HasKey(bd => bd.Id);

            builder.HasMany(bd => bd.UserBankDetails)
                .WithOne(ubd => ubd.BankDetails);

            builder.Property(bd => bd.Number)
                .IsRequired(true);

            builder.Property(bd => bd.Cvv)
                .IsRequired(true);

            builder.Property(bd => bd.ExpiryDate)
                .IsRequired(true);

            builder.Property(bd => bd.ModifiedOn);
            builder.Property(bd => bd.CreatedOn);
            builder.Property(bd => bd.DeletedOn);
            builder.Property(bd => bd.IsDeleted);
        }
    }
}
