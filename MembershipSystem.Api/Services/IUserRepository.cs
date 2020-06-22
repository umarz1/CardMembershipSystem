using MembershipSystem.Api.Models;

namespace MembershipSystem.Api.Services
{
    public interface IUserRepository
    {
        User GetUserByCardId(string cardId);
        void AddUser(User user);
    }
}
