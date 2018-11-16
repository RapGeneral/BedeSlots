﻿// <auto-generated />
using System;
using BedeSlots.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BedeSlots.DataContext.Migrations
{
    [DbContext(typeof(BedeDbContext))]
    [Migration("20181116111849_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BedeSlots.DataModels.Balance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CurrencyId");

                    b.Property<decimal>("Money");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UserId");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("BedeSlots.DataModels.BankDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cvv");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<string>("Number");

                    b.HasKey("Id");

                    b.ToTable("BankDetails");
                });

            modelBuilder.Entity("BedeSlots.DataModels.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CurrencyName");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("BedeSlots.DataModels.Rate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BaseCurrencyId");

                    b.Property<decimal>("Coeff");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("ToCurrencyId");

                    b.HasKey("Id");

                    b.HasIndex("BaseCurrencyId");

                    b.HasIndex("ToCurrencyId");

                    b.ToTable("Rates");
                });

            modelBuilder.Entity("BedeSlots.DataModels.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<Guid?>("BalanceId1");

                    b.Property<Guid?>("BalanceIdId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<decimal>("OpeningBalance");

                    b.Property<Guid>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId1");

                    b.HasIndex("BalanceIdId");

                    b.HasIndex("TypeId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BedeSlots.DataModels.TransactionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("TransactionTypes");
                });

            modelBuilder.Entity("BedeSlots.DataModels.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<Guid?>("CurrencyId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<Guid>("UserCurrencyId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BedeSlots.DataModels.UserBankDetails", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<Guid>("BankDetailsId");

                    b.HasKey("UserId", "BankDetailsId");

                    b.HasIndex("BankDetailsId");

                    b.ToTable("UserBankDetails");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BedeSlots.DataModels.Balance", b =>
                {
                    b.HasOne("BedeSlots.DataModels.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BedeSlots.DataModels.User", "User")
                        .WithMany("Balance")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("BedeSlots.DataModels.Rate", b =>
                {
                    b.HasOne("BedeSlots.DataModels.Currency", "BaseCurrency")
                        .WithMany()
                        .HasForeignKey("BaseCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BedeSlots.DataModels.Currency", "ToCurrency")
                        .WithMany("Rates")
                        .HasForeignKey("ToCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BedeSlots.DataModels.Transaction", b =>
                {
                    b.HasOne("BedeSlots.DataModels.Balance", "Balance")
                        .WithMany()
                        .HasForeignKey("BalanceId1");

                    b.HasOne("BedeSlots.DataModels.Balance", "BalanceId")
                        .WithMany()
                        .HasForeignKey("BalanceIdId");

                    b.HasOne("BedeSlots.DataModels.TransactionType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BedeSlots.DataModels.User", b =>
                {
                    b.HasOne("BedeSlots.DataModels.Currency", "Currency")
                        .WithMany("Users")
                        .HasForeignKey("CurrencyId");
                });

            modelBuilder.Entity("BedeSlots.DataModels.UserBankDetails", b =>
                {
                    b.HasOne("BedeSlots.DataModels.BankDetails", "BankDetails")
                        .WithMany("UserBankDetails")
                        .HasForeignKey("BankDetailsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BedeSlots.DataModels.User", "User")
                        .WithMany("UserBankDetails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BedeSlots.DataModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BedeSlots.DataModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BedeSlots.DataModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BedeSlots.DataModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
