namespace MembershipSystem.Api.Services.Queries
{
    public class CommandText : ICommandText
    {
        public string GetMemberByCardId => @"SELECT m.EmployeeId, m.Name, m.Email, m.Mobile
                                          FROM Members m 
                                          JOIN Cards c ON m.EmployeeId= c.EmployeeId 
                                          WHERE c.CardId = @CardId";

        public string AddMember => @"INSERT INTO Members (EmployeeId, Name, Email, Mobile)
                                   VALUES (@EmployeeId, @Name, @Email, @Mobile)";

        public string AddCard => @"INSERT INTO Cards (CardId, EmployeeId, Pin)
                                   VALUES (@CardId, @EmployeeId, @Pin)";

        public string GetLatestCardTransactionByCardId => @"SELECT TOP(1) ct.TransactionId, ct.CardId, ct.Date, ct.Amount, ct.Balance
                                          FROM CardTransactions ct 
                                          WHERE ct.CardId = @CardId
                                          ORDER BY Date DESC";

        public string AddAmountToCard => @"DECLARE @CurrentBalance Decimal;
                                           SET @CurrentBalance = (SELECT TOP(1) Balance from CardTransactions where CardId= @CardId ORDER BY Date DESC)
                                           
                                           IF @CurrentBalance IS NULL
                                                 INSERT INTO CardTransactions(TransactionId, CardId, Date, Amount, Balance)
                                                 VALUES(@TransactionId, @CardId, GETDATE(), @Amount, @Amount)
                                           ELSE 
                                                 INSERT INTO CardTransactions(TransactionId, CardId, Date, Amount, Balance)
                                                 VALUES(@TransactionId, @CardId, GETDATE(), @Amount, @CurrentBalance + @Amount)";
    }
}
