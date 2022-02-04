using System.Data;
using System.Linq;
using Todos.Application.Interfaces;

namespace Todos.Application.Extensions
{
    public static class DbDataReaderExtensions
    {
        public static bool HasColumn(this IDataReaderWrapper reader, string columnName)
        {
            var schema = reader.GetSchemaTable();
            return schema is { } && schema.Rows.Cast<DataRow>().Any(row => row["ColumnName"].ToString() == columnName);
        }
    }
}