namespace MembershipSystem.Api.Services.Queries
{
    public interface IMembersCommandText
    {
        string GetMemberByCardId { get; }
        string AddMember { get; }

        string AddCard { get; }
    }
}
