using System;
using System.Data.SqlClient;

namespace MembershipSystem.Api.Services.ExecuteCommands
{
    public interface IExecuters
    {
        void ExecuteCommand(string connStr, Action<SqlConnection> task);
        T ExecuteCommand<T>(string connStr, Func<SqlConnection, T> task);

    }
}
