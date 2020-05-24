using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FYP_WEB_APP.Models
{
    public class DBManger
    {

        public String connectionStr;
        private String dbName = "FYP_1920";
        public MongoClient dbClient;
        public IMongoDatabase DataBase;

        private string UKdbName = "mydb";
        private string UKConnectionString = "mongodb://hkteam1:IUXsr2ZYKQuPu0Sj@issf2020hk-shard-00-00-la5xb.gcp.mongodb.net:27017,issf2020hk-shard-00-01-la5xb.gcp.mongodb.net:27017,issf2020hk-shard-00-02-la5xb.gcp.mongodb.net:27017/test?ssl=true&replicaSet=ISSF2020HK-shard-0&authSource=admin&retryWrites=true&w=majority";
        public MongoClient UKdbClient ;
        public IMongoDatabase Weatherdatabase;

        public DBManger()
        {
            this.connectionStr = ("mongodb://admin:admin@clustertest-shard-00-00-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-01-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-02-kjhvv.azure.mongodb.net:27017/test?ssl=true&replicaSet=ClusterTest-shard-0&authSource=admin&retryWrites=true&w=majority");
            this.dbClient = new MongoClient(this.connectionStr);
            //var database = dbClient.GetDatabase(this.dbName);
            this.DataBase = dbClient.GetDatabase(this.dbName);
            //var collection = database.GetCollection<MongoLightListModel>("LIGHT_LIST");
            this.UKdbClient = new MongoClient(UKConnectionString);
            this.Weatherdatabase = UKdbClient.GetDatabase(UKdbName);
        }

 
		
    }
}
