

using BedeSlots.Infrastructure.Providers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BedeSlots.Infrastructure.Providers
{
    public class SignInManagerWrapper<T> : ISignInManager<T> where T: class
    {
        private SignInManager<T> _signInManager;

        public SignInManagerWrapper(SignInManager<T> _signInManager)
        {
            this._signInManager = _signInManager;
        }
        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }
        public async Task SignInAsync(T user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
