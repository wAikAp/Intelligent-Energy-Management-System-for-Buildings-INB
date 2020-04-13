using System;
using MongoDB.Driver;

namespace FYP_WEB_APP.Models
{
    public class DBManger
    {

        public String connectionStr;
        private String dbName = "FYP_1920";
        public MongoClient dbClient;
        public IMongoDatabase DataBase;

        public DBManger()
        {
            this.connectionStr = ("mongodb://admin:admin@clustertest-shard-00-00-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-01-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-02-kjhvv.azure.mongodb.net:27017/test?ssl=true&replicaSet=ClusterTest-shard-0&authSource=admin&retryWrites=true&w=majority");
            this.dbClient = new MongoClient(this.connectionStr);
            //var database = dbClient.GetDatabase(this.dbName);
            this.DataBase = dbClient.GetDatabase(this.dbName);
            //var collection = database.GetCollection<MongoLightListModel>("LIGHT_LIST");


        }
    }
}
