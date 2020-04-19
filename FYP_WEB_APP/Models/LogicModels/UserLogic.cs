using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;


namespace FYP_APP.Models.LogicModels
{
	public class UserLogic
	{
		string userName,pwd;
		DBManger dbManager;

		
		public UserLogic()
		{
			dbManager = new DBManger();
		}

		public MongoUserModel login(string un ,string pd)
		{
			this.userName = un;
			this.pwd = pd;

			var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");

			var filterBuilder = Builders<MongoUserModel>.Filter;
			var filter = filterBuilder.Eq("userName", userName) & filterBuilder.Eq("password", pwd);
			var document = collection.Find(filter).FirstOrDefault();
			if(document != null)
			{
				return document;
			}
			else
			{
				Debug.WriteLine("not find");
				return null;
			}



			{
				/*
			 if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
			{
				HttpContext.Session.SetString(SessionKeyName, "The Doctor");
				HttpContext.Session.SetInt32(SessionKeyAge, 773);
			}

			var name = HttpContext.Session.GetString(SessionKeyName);
			var age = HttpContext.Session.GetInt32(SessionKeyAge);
		*/
			}
		}
	}
}
