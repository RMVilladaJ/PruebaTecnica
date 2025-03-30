using Microsoft.AspNetCore.Identity;
using PruebaTecnica.Shared.DTOs;
using PruebaTecnica.Shared.Entidades;


namespace PruebaTecnica.API.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);


    }
}
