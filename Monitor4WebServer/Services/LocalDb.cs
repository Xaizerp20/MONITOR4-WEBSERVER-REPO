using LiteDB.Async;
using LiteDB.Async;
using Monitor4WebServer.Data;
using System.Threading;
using System.Xml.Linq;

namespace Monitor4WebServer.Services
{

    public class LocalDb : IDisposable
    {
        private LiteDatabaseAsync _database;
        private LiteCollectionAsync<LineReport> _collection;


        public LocalDb()
        {
            _database = new LiteDatabaseAsync(@"C:\Users\Informatica\Desktop\MONITOR4LR\MONITOR4_LR.db");
            _collection = (LiteCollectionAsync<LineReport>?)_database.GetCollection<LineReport>("LineasReporte");

        }

        public async Task<List<LineReport>> GetAllDataAsync()
        {
            return await _collection.Query().ToListAsync();
        }

        public async Task InsertDataAsync(LineReport data)
        {
            await _collection.InsertAsync(data);
        }

        public async Task UpdateDataAsync(LineReport data)
        {
            await _collection.UpdateAsync(data);
        }

        public async Task CommitAsync()
        {
            await _database.CommitAsync();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
