using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BedeSlots.Infrastructure.Providers.Interfaces
{
    public interface ISignInManager<T> where T : class
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignInAsync(T user, bool isPersistent);
        Task SignOutAsync();
    }
}
