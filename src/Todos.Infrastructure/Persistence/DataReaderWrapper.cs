using System.Data;
using Todos.Application.Interfaces;

namespace Todos.Infrastructure.Persistence
{
    public class DataReaderWrapper : IDataReaderWrapper
    {
        private readonly IDataReader _reader;
        
        public DataReaderWrapper(IDataReader reader)
        {
            _reader = reader;
        }
        
        public virtual object GetValue(string name) => _reader[name];
        
        public DataTable GetSchemaTable()
        {
            return _reader.GetSchemaTable();
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}