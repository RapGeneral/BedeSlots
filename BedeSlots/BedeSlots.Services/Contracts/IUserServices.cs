using BedeSlots.ViewModels.GlobalViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Contracts
{
    public interface IUserServices
    {
        Task<ICollection<UserViewModel>> SearchByUsernameAsync(string username);
    }
}
