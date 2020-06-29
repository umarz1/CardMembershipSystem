namespace MembershipSystem.Api.Services.Queries
{
    public class MembersCommandText : IMembersCommandText
    {
        public string GetMemberByCardId => @"SELECT m.EmployeeId, m.Name, m.Email, m.Mobile
                                          FROM Members m 
                                          JOIN Cards c ON m.EmployeeId= c.EmployeeId 
                                          WHERE c.CardId = @CardId";

        public string AddMember => @"INSERT INTO Members (EmployeeId, Name, Email, Mobile)
                                   VALUES (@EmployeeId, @Name, @Email, @Mobile)";

        public string AddCard => @"INSERT INTO Cards (CardId, EmployeeId, Pin)
                                   VALUES (@CardId, @EmployeeId, @Pin)";
    }
}
