using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.bussiness.Repository
{
    public interface IMDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name); 
    }
}
