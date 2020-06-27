using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace MembershipSystem.Api.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMembershipRepository _membershipRepo;

        public MemberService(IMembershipRepository membershipRepo)
        {
            _membershipRepo = membershipRepo;
        }

        MemberDto IMemberService.GetMember(string cardId)
        {
            var member = _membershipRepo.GetMember(cardId);
            return member;
        }

        public MemberDto AddMember(NewMember member)
        {
            var addMember = _membershipRepo.AddMember(member);
            return addMember;
        }
    }
}
