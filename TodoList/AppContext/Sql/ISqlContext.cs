using System.Data;

namespace TodoListAPI.AppContext.Sql
{
    public interface ISqlContext
    {
        IDbConnection CreateConnection();
    }
}
