using System;
using System.Data;

namespace Todos.Application.Interfaces
{
    public interface IDataReaderWrapper : IDisposable
    {
        object GetValue(string name);
        DataTable GetSchemaTable();
        bool Read();
    }
}