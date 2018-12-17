using BedeSlots.DataContext;
using BedeSlots.DataModels;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BedeDBContext>();
            context.Database.EnsureCreated();

            string[] roles = Enum.GetNames(typeof(UserRoles));

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            var userToAdd = new User
            {
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin.admin@gmail.com",
                NormalizedEmail = "ADMIN.ADMIN@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEGmEntoTdhbreTdb0rM745Ml22uMtO4VgjNTPdDaOERfySDw2LxqADe0ZgK1Nmc7eg==",
                SecurityStamp = "62Y6TC37XC6CMQBTH3RFWUJVINTQ6CR2",
                ConcurrencyStamp = "308d3213-6583-4baf-a672-b51dd744dc89",
                DateOfBirth = new DateTime(1999, 2, 10),
                CreatedOn = new DateTime(2018, 12, 17, 11, 27, 53)
            };

            if (!context.Users.Any(u => u.UserName == userToAdd.UserName))
            {
                var userStore = new UserStore<User>(context);
                var result = userStore.CreateAsync(userToAdd);

                var userServices = serviceProvider.GetRequiredService<IUserServices>();
                userServices.CreateUserInitialBalances(userToAdd.Id, "EUR").Wait();
            }

            AssignRoles(serviceProvider, userToAdd.UserName, roles).Wait();

            context.SaveChanges();

            if (!context.TransactionTypes.Any())
            {
                context.TransactionTypes.Add(new TransactionType { Name = TypeOfTransaction.Deposit.ToString() });
                context.TransactionTypes.Add(new TransactionType { Name = TypeOfTransaction.Win.ToString() });
                context.TransactionTypes.Add(new TransactionType { Name = TypeOfTransaction.Stake.ToString() });
            }
            else if (context.TransactionTypes.Count() != 3)
            {
                throw new ArgumentException("Transaction types count is different thatn 3 but not 0!");
            }

            if (!context.BalanceTypes.Any())
            {
                context.BalanceTypes.Add(new BalanceType { Name = BalanceTypes.Base.ToString() });
                context.BalanceTypes.Add(new BalanceType { Name = BalanceTypes.Bonus.ToString() });
                context.BalanceTypes.Add(new BalanceType { Name = BalanceTypes.Personal.ToString() });
            }
            else if (context.BalanceTypes.Count() != 3)
            {
                throw new ArgumentException("Balance types count is different thatn 3 but not 0!");
            }

        }

        private static async Task<IdentityResult> AssignRoles(IServiceProvider services, string id, string[] roles)
        {
            UserManager<User> _userManager = services.GetService<UserManager<User>>();
            User user = await _userManager.FindByNameAsync(id);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}
