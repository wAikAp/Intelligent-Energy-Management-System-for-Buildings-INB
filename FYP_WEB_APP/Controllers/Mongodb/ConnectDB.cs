using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Controllers.Mongodb
{
    public class ConnectDB
    {
        public IMongoDatabase Conn()
        {
            var connectionString = ("mongodb://admin:admin@clustertest-shard-00-00-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-01-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-02-kjhvv.azure.mongodb.net:27017/test?ssl=true&replicaSet=ClusterTest-shard-0&authSource=admin&retryWrites=true&w=majority");

            MongoClient dbClient = new MongoClient(connectionString);
            IMongoDatabase database = dbClient.GetDatabase("FYP_1920");

            return database;
        }
    }
}
