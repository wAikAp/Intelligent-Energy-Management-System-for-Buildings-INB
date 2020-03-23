using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using FYP_WEB_APP.Models.MongoModels;
using System.Diagnostics;


namespace FYP_WEB_APP.Controllers
{
	public class DevicesController : Controller
	{
		public IActionResult Devices()
		{

			
			System.Diagnostics.Debug.WriteLine("Hello..........");
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("");

			/*var connectionString = "mongodb+srv://admin:admin@clustertest-kjhvv.azure.mongodb.net/test?retryWrites=true&w=majority";
			MongoClient dbClient = new MongoClient(connectionString);
			var database = dbClient.GetDatabase("FYP_1920");
			var collection = database.GetCollection<MongoLightListModel>("LIGHT_LIST");

			var documents = collection.Find(new BsonDocument()).ToList();
			foreach(MongoLightListModel ll in documents)
			{
				Debug.WriteLine(ll.lastest_checking_time);
			}*/


			return View();
		}
	}
}