using BedeSlots.DataContext.Repository;
using BedeSlots.DataModels;
using BedeSlots.Infrastructure.MappingProvider;
using BedeSlots.Services.Contracts;
using BedeSlots.ViewModels.GlobalViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMappingProvider mappingProvider;
        private readonly IRepository<User> userRepo;

        public UserServices(IRepository<User> userRepo, IMappingProvider mappingProvider)
        {
            this.mappingProvider = mappingProvider;
            this.userRepo = userRepo;
        }
        public async Task<ICollection<UserViewModel>> SearchByUsernameAsync(string username)
        {
            List<User> users;
            if (username is null)
            {
                users = await userRepo.All().ToListAsync();
            }
            else
            {
                users = await userRepo.All().Where(u => u.UserName.ToLower()
                                    .Contains(username.ToLower()))
                                    .ToListAsync();
            }
            var models = mappingProvider.MapTo<ICollection<UserViewModel>>(users);
            return models;
        }
    }
}
