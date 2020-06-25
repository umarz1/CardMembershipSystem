namespace MembershipSystem.Api.Services.Queries
{
    public interface ICommandText
    {
        string GetEmployeeByCardId { get; }
        string AddEmployee { get; }

        string AddCard { get; }
    }
}
