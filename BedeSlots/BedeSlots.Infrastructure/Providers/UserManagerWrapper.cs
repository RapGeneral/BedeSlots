using BedeSlots.Infrastructure.Providers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Infrastructure.Providers
{

    public class UserManagerWrapper<T> : IUserManager<T> where T : class
    {
        private UserManager<T> _userManager;

        public UserManagerWrapper(UserManager<T> userManager)
        {
            _userManager = userManager;
        }

        public UserManager<T> Instance => _userManager;
        public IQueryable<T> Users => _userManager.Users;
        public IList<IPasswordValidator<T>> PasswordValidators => _userManager.PasswordValidators;

        public async Task<IdentityResult> SetLockoutEndDateAsync(T user, DateTimeOffset? lockoutEnd)
        {
            return await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        }
        public async Task<IdentityResult> SetLockoutEnabledAsync(T user, bool enabled)
        {
            return await _userManager.SetLockoutEnabledAsync(user, enabled);
        }
        public async Task<IdentityResult> RemovePasswordAsync(T user)
        {
            return await _userManager.RemovePasswordAsync(user);
        }

        public async Task<IdentityResult> AddPasswordAsync(T user, string password)
        {
            return await _userManager.AddPasswordAsync(user, password);
        }

        public async Task<string> GetUserIdAsync(T user)
        {
            return await _userManager.GetUserIdAsync(user);
        }
        public async Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<IdentityResult> AddToRoleAsync(T user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }
        public async Task<IdentityResult> RemoveFromRoleAsync(T user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }
        public string GetUserId(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }
        public async Task<IdentityResult> CreateAsync(T user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
