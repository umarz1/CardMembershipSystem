using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;

namespace MembershipSystem.Api.Services
{
    public interface IMembershipRepository
    {
        MemberDto GetMember(string cardId);
        MemberDto AddMember(NewMember member);
    }
}
