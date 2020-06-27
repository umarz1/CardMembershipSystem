using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Services
{
    public interface IMemberService 
    {
        MemberDto GetMember(string cardId);
        MemberDto AddMember(NewMember member);
    }
}
