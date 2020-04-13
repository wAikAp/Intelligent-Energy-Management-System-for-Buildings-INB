using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP_APP.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using FYP_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http;


namespace FYP_APP.Controllers
{
	public class HomeController : Controller
	{

		


		public IActionResult Index()
		{

			HttpContext.Session.SetString("code", "123456");


			//test
			/*var connectionString = "mongodb+srv://admin:admin@clustertest-kjhvv.azure.mongodb.net/test?retryWrites=true&w=majority";
			MongoClient dbClient = new MongoClient(connectionString);
			var database = dbClient.GetDatabase("FYP_1920");
			var collection = database.GetCollection<BsonDocument>("USER");


			//insert
			/*var document = new BsonDocument { { "student_id", 10000 }, {
				"scores",
				new BsonArray {
				new BsonDocument { { "type", "exam" }, { "score", 88.12334193287023 } },
				new BsonDocument { { "type", "quiz" }, { "score", 74.92381029342834 } },
				new BsonDocument { { "type", "homework" }, { "score", 89.97929384290324 } },
				new BsonDocument { { "type", "homework" }, { "score", 82.12931030513218 } }
				}
				}, { "class_id", 480 }
			};

			collection.InsertOne(document);
			//Or 
			//collection.InsertOneAsync(document);


			//end insert



			/*var dbList = dbClient.ListDatabases().ToList();

			Console.WriteLine("The list of databases on this server is: .............asdhgiagriarb");
			foreach (var db in dbList)
			{
				Console.WriteLine(db);
			}*/


			

			return View();
		}

		public IActionResult Home(string ac, string pwd)

		{
			return View();
		}

		public IActionResult Home()
		{

			
			return View();
		}

		


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
