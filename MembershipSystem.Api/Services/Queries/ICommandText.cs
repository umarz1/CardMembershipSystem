namespace MembershipSystem.Api.Services.Queries
{
    public interface ICommandText
    {
        string GetMemberByCardId { get; }
        string AddMember { get; }

        string AddCard { get; }
    }
}
