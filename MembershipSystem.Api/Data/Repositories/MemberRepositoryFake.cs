using System.Linq;
using MembershipSystem.Api.DTOs;
using MembershipSystem.Api.Models;
using System.Collections.Generic;

namespace MembershipSystem.Api.Services
{
    public class MemberRepositoryFake : IMembershipRepository
    {

        public Dictionary<string, Member> members = new Dictionary<string, Member>()
        {
            {"9997", new Member{ MemberId="9997", Name= "Test User1", Email = "test.user1@hotmail.com", Mobile ="07745314222"} },
            {"9998", new Member{ MemberId="9998", Name = "Test User2", Email = "test.user2@hotmail.com", Mobile ="07745314222"} },
            {"9999", new Member{ MemberId="9999", Name = "Test User2", Email = "test.user2@hotmail.com", Mobile ="07745314222"} } 
        };

        public Dictionary<string, Card> cards = new Dictionary<string, Card>()
        {
            {"VyDJ0lbYcPkzp2Ju", new Card{ CardId ="VyDJ0lbYcPkzp2Ju",  MemberId="9997", Pin= "1234"} },
            {"2sJ6fY4lYJaZFX9w", new Card{ CardId ="2sJ6fY4lYJaZFX9w",  MemberId="9998", Pin= "1235"} },
            {"Bl5vKfTP7Q7jW4PH", new Card{ CardId ="Bl5vKfTP7Q7jW4PH",  MemberId="9999", Pin= "1236"} },
        };

        public MemberDto GetMember(string cardId)
        {
            var card = cards.Where(c => c.Key == cardId).FirstOrDefault();

            if(card.Value == null)
            {
                return null;
            }

            var member = members.Where(m => m.Value.MemberId == card.Value.MemberId).FirstOrDefault();

            return new MemberDto { Name = member.Value.Name };
        }

        public MemberDto AddMember(NewMember newMember)
        {
            var member = new Member()
            {
                MemberId = newMember.MemberId,
                Name = newMember.Name,
                Email = newMember.Email,
                Mobile = newMember.Mobile
            };

            var card = new Card()
            {
                CardId = newMember.CardId,
                MemberId = newMember.MemberId,
                Pin = newMember.Pin
            };

            if (members.TryAdd(member.MemberId, member) && cards.TryAdd(newMember.CardId, card))
            {
                return new MemberDto { Name = member.Name };
            }
            else
            {
                return null;
            }
        }
    }
}
