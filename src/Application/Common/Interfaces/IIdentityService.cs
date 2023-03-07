using Zemoga_Test.Application.Common.Models;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<IEnumerable<Role>> GetRolesAsync(string userId);
}
