namespace MembershipSystem.Api.Services.Queries
{
    public class CommandText : ICommandText
    {
        public string GetMemberByCardId => @"SELECT e.EmployeeId, e.Name, e.Email, e.Mobile
                                          FROM Employees e 
                                          JOIN Cards c ON e.EmployeeId= c.EmployeeId 
                                          WHERE c.CardId = @CardId";

        public string AddMember => @"INSERT INTO Members (EmployeeId, Name, Email, Mobile)
                                   VALUES (@EmployeeId, @Name, @Email, @Mobile)";

        public string AddCard => @"INSERT INTO Cards (CardId, EmployeeId, Pin)
                                   VALUES (@CardId, @EmployeeId, @Pin)";
    }
}
