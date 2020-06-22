﻿namespace MembershipSystem.Api.Services.Queries
{
    public class CommandText : ICommandText
    {
        public string GetUserByCardId => "Select * from CardUsers where CardId= @CardId";

        public string AddUser => "INSERT INTO CardUsers (CardId, EmployeeId, Name, Email, Mobile) " +
            "VALUES ('@CardId', @EmployeeId, @Name, @Email, @Mobile) ";
    }
}
