namespace MembershipSystem.Api.Services.Queries
{
    public class CommandText : ICommandText
    {
        public string GetUserByCardId => @"SELECT e.EmployeeId, e.Name, e.Email, e.Mobile
                                          FROM Employees e 
                                          JOIN Cards c ON e.EmployeeId= c.EmployeeId 
                                          WHERE c.CardId = @CardId";

        public string AddUser => @"INSERT INTO Employees (EmployeeId, Name, Email, Mobile)
                                   VALUES (@EmployeeId, @Name, @Email, @Mobile)";

        public string AddCard => @"INSERT INTO Cards (CardId, EmployeeId, Pin)
                                   VALUES (@CardId, @EmployeeId, @Pin)";
    }
}
