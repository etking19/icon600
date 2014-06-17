
namespace Sqlite.Data
{
    public interface ISqlData
    {
        string GetCreateCommand();
        string GetAddCommand();
        string GetRemoveCommand();

        string GetTableName();

        string GetQueryCommand();
        string GetUpdateDataCommand();
    }
}
