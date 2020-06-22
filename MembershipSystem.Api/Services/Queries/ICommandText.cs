namespace MembershipSystem.Api.Services.Queries
{
    public interface ICommandText
    {
        string GetUserByCardId { get; }
        string AddUser { get; }
    }
}
