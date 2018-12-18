using BedeSlots.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.DataContext.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(bt => bt.Id);

            builder.HasMany(u => u.Balances)
                .WithOne(b => b.User)
                .IsRequired();

            builder.HasMany(u => u.UserBankDetails)
                .WithOne(ubd => ubd.User);

            builder.Property(u => u.DateOfBirth)
                .IsRequired(true);

            builder.Property(u => u.CreatedOn)
                .IsRequired(true);

            builder.Property(u => u.ModifiedOn);
        }
    }
}
