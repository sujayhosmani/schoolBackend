using jay.school.contracts.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.bussiness.Repository
{
    public class SchoolMDBContext : IMDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public SchoolAppsettings schoolAppsettings { get; set; } 
        public SchoolMDBContext(IOptions<SchoolAppsettings> configuration)
        {
             schoolAppsettings = configuration.Value;
            _mongoClient = new MongoClient(schoolAppsettings.MangoDbConnection.ConnectionString);
            _db = _mongoClient.GetDatabase(schoolAppsettings.MangoDbConnection.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);
    }
}

