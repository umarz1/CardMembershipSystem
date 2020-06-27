namespace MembershipSystem.Api.Services.Queries
{
    public class TransactionsCommandText : ITransactionsCommandText
    {
        public string AddCard => @"INSERT INTO Cards (CardId, EmployeeId, Pin)
                                   VALUES (@CardId, @EmployeeId, @Pin)";

        public string GetLatestCardTransactionByCardId => @"SELECT TOP(1) ct.TransactionId, ct.CardId, ct.Date, ct.Amount, ct.Balance
                                          FROM Transactions ct 
                                          WHERE ct.CardId = @CardId
                                          ORDER BY Date DESC";

        public string AddAmountToCard => @"DECLARE @CurrentBalance Decimal;
                                           SET @CurrentBalance = (SELECT TOP(1) Balance from Transactions where CardId= @CardId ORDER BY Date DESC)
                                           
                                           IF @CurrentBalance IS NULL
                                                 INSERT INTO Transactions(TransactionId, CardId, Date, Amount, Balance)
                                                 VALUES(@TransactionId, @CardId, GETDATE(), @Amount, @Amount)
                                           ELSE 
                                                 INSERT INTO Transactions(TransactionId, CardId, Date, Amount, Balance)
                                                 VALUES(@TransactionId, @CardId, GETDATE(), @Amount, @CurrentBalance + @Amount)";
    }
}
