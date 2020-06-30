namespace MembershipSystem.Api.Services.Queries
{
    public class MembersCommandText : IMembersCommandText
    {
        public string GetMemberByCardId => @"SELECT m.MemberId, m.Name, m.Email, m.Mobile
                                          FROM Members m 
                                          JOIN Cards c ON m.MemberId= c.MemberId 
                                          WHERE c.CardId = @CardId";

        public string AddMember => @"INSERT INTO Members (MemberId, Name, Email, Mobile)
                                   VALUES (@MemberId, @Name, @Email, @Mobile)";

        public string AddCard => @"INSERT INTO Cards (CardId, MemberId, Pin)
                                   VALUES (@CardId, @MemberId, @Pin)";
    }
}
