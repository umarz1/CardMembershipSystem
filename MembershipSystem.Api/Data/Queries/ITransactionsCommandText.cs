namespace MembershipSystem.Api.Services.Queries
{
    public interface ITransactionsCommandText
    {
        string GetLatestCardTransactionByCardId { get; }

        string AddAmountToCard { get; }
    }
}
