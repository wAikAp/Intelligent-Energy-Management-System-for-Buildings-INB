using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using FYP_WEB_APP.Models.MongoModels;


namespace FYP_WEB_APP.Controllers
{
	public class DevicesController : Controller
	{
		public IActionResult Devices()
		{
			TempData["Category"] = "test";


			/*
			Debug.WriteLine("Hello..........");
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("");
			*/
			var connectionString = ("mongodb://admin:admin@clustertest-shard-00-00-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-01-kjhvv.azure.mongodb.net:27017,clustertest-shard-00-02-kjhvv.azure.mongodb.net:27017/test?ssl=true&replicaSet=ClusterTest-shard-0&authSource=admin&retryWrites=true&w=majority");

			//var connectionString = "mongodb+srv://admin:admin@clustertest-kjhvv.azure.mongodb.net/test?retryWrites=true&w=majority";
			MongoClient dbClient = new MongoClient(connectionString);
			var database = dbClient.GetDatabase("FYP_1920");
			var collection = database.GetCollection<MongoLightListModel>("LIGHT_LIST");

			var documents = collection.Find(new BsonDocument()).ToList();
			var datalist = new List<MongoLightListModel> { };

			foreach (MongoLightListModel ll in documents)
			{
				//Debug.WriteLine(ll.lastest_checking_time);
				//Debug.WriteLine(ll.roomId);
				var x = new MongoLightListModel()
				{
					roomId = ll.roomId
				};
				datalist.Add(x);
			}


			return View(datalist);
		}
	}
}