namespace MembershipSystem.Api.Services.Queries
{
    public interface ITransactionsCommandText
    {
        string AddCard { get; }

        string GetLatestCardTransactionByCardId { get; }

        string AddAmountToCard { get; }
    }
}
