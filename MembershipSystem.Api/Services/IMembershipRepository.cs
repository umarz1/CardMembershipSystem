using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;

namespace MembershipSystem.Api.Services
{
    public interface IMembershipRepository
    {
        EmployeeDto GetMember(string cardId);
        EmployeeDto AddMember(Member member);
    }
}
