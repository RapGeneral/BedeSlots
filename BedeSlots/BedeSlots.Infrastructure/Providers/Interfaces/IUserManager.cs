using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Infrastructure.Providers.Interfaces
{
    public interface IUserManager<T> where T : class
    {
        Task<IdentityResult> SetLockoutEndDateAsync(T usrer, DateTimeOffset? lockoutEnd);
        Task<IdentityResult> SetLockoutEnabledAsync(T user, bool enabled);
        Task<IdentityResult> AddPasswordAsync(T user, string password);
        Task<IdentityResult> RemovePasswordAsync(T user);
        Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<string> GetUserIdAsync(T user);
        IQueryable<T> Users { get; }
        IList<IPasswordValidator<T>> PasswordValidators { get; }
        UserManager<T> Instance { get; }
        Task<IdentityResult> AddToRoleAsync(T user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(T user, string role);
        string GetUserId(ClaimsPrincipal principal);
        Task<IdentityResult> CreateAsync(T user, string password);
    }
}
