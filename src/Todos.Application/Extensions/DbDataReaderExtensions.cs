using System.Data;
using System.Linq;

namespace Todos.Application.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static bool HasColumn(this IDataReader reader, string columnName)
        {
            var schema = reader.GetSchemaTable();
            return schema is { } && schema.Rows.Cast<DataRow>().Any(row => row["ColumnName"].ToString() == columnName);
        }
    }
}