using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;

namespace MembershipSystem.Api.Services
{
    public interface IUserRepository
    {
        UserDto GetUserByCardId(string cardId);
        UserDto AddUser(User user);
    }
}
