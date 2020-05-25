using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Models;
using FYP_WEB_APP.Models.MongoModels;
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
		}

		public MongoUserModel getUserModel(string userName)
		{
			var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
			var filterBuilder = Builders<MongoUserModel>.Filter;
			var filter = filterBuilder.Eq("userName", userName);
			var document = collection.Find(filter).FirstOrDefault();
			if (document != null)
			{
				return document;
			}
			else
			{
				Debug.WriteLine("not find");
				return null;
			}
		}

		public List<MongoUserModel> getAllUserModel()
		{
			var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
			try
			{
				var documentList = collection.Find(new BsonDocument()).ToList();
				if (documentList != null)
				{
					foreach (MongoUserModel document in documentList)
					{
						Debug.WriteLine("find user: " + document.userName);
					}
					return documentList;
				}
				else
				{
					Debug.WriteLine("not find");
					return null;
				}
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
				return null;
			}
			
		}

		public List<MongoPersonalUsagePlanModel> getPersonalUsagePlan(String userName)
		{
			var collection = dbManager.DataBase.GetCollection<MongoPersonalUsagePlanModel>("PERSONAL_USAGE_PLAN");
			var filterBuilder = Builders<MongoPersonalUsagePlanModel>.Filter;
			var filter = filterBuilder.Eq("userName", userName);
			List<MongoPersonalUsagePlanModel> documentList = collection.Find(filter).ToList();
			foreach (MongoPersonalUsagePlanModel m in documentList)
			{
				Debug.WriteLine("m :" + m.userName);
			}
			if (documentList != null)
			{
				return documentList;
			}
			else
			{
				Debug.WriteLine("not find");
				return null;
			}
		}

		public Boolean updatePersonalUsagePlan(String userName, double pTemp, double pHum, double pLig, String desc, String name)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoPersonalUsagePlanModel>("PERSONAL_USAGE_PLAN");
				var filterBuilder = Builders<MongoPersonalUsagePlanModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName) & filterBuilder.Eq("name", name);

				var update = Builders<MongoPersonalUsagePlanModel>.Update.Set("pTemp", pTemp).Set("pHum", pHum).Set("pLig", pLig).Set("desc", desc);
				collection.UpdateOne(filter, update);
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
			

			return true;
		}

		public Boolean createPersonalUsagePlan(String userName, double pTemp, double pHum, double pLig, String desc, String name)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoPersonalUsagePlanModel>("PERSONAL_USAGE_PLAN");
				var filterBuilder = Builders<MongoPersonalUsagePlanModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName) & filterBuilder.Eq("name", name);
				var Document = collection.Find(filter).FirstOrDefault();
				if(Document == null)
				{
					MongoPersonalUsagePlanModel mongoPersonalUsagePlanModel = new MongoPersonalUsagePlanModel();
					mongoPersonalUsagePlanModel.userName = userName;
					mongoPersonalUsagePlanModel.pTemp = pTemp;
					mongoPersonalUsagePlanModel.pHum = pHum;
					mongoPersonalUsagePlanModel.pLig = pLig;
					mongoPersonalUsagePlanModel.desc = desc;
					mongoPersonalUsagePlanModel.name = name;
					collection.InsertOne(mongoPersonalUsagePlanModel);
					return true;
					//Debug.WriteLine("Document "+ Document.name);
				}
				
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}

			return false;
		}

		public Boolean deletePersonalUsagePlan(String userName, String name)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoPersonalUsagePlanModel>("PERSONAL_USAGE_PLAN");
				var filterBuilder = Builders<MongoPersonalUsagePlanModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName) & filterBuilder.Eq("name", name);
				collection.DeleteOne(filter);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}

		public Boolean updateUserPassword(String userName, String password)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
				var filterBuilder = Builders<MongoUserModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName);
				var update = Builders<MongoUserModel>.Update.Set("password", password);
				collection.UpdateOne(filter, update);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}

		public Boolean updateUserInfo(String userName, String fName, String lName)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
				var filterBuilder = Builders<MongoUserModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName);
				var update = Builders<MongoUserModel>.Update.Set("fName", fName).Set("lName", lName);
				collection.UpdateOne(filter, update);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}

		public Boolean adminAddUser(String userName, String fName, String lName, String password)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
				var filterBuilder = Builders<MongoUserModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName);
				var Document = collection.Find(filter).FirstOrDefault();
				if (Document == null)
				{
					MongoUserModel mongoUserModel = new MongoUserModel();
					mongoUserModel.userName = userName;
					mongoUserModel.fName = fName;
					mongoUserModel.lName = lName;
					mongoUserModel.password = password;
					collection.InsertOne(mongoUserModel);
					return true;
					//Debug.WriteLine("Document "+ Document.name);
				}

			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}

			return false;
		}

		public Boolean adminUpdateUserInfo(String userName, String fName, String lName, String password)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
				var filterBuilder = Builders<MongoUserModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName);
				var update = Builders<MongoUserModel>.Update.Set("fName", fName).Set("lName", lName).Set("password", password);
				collection.UpdateOne(filter, update);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}

		public Boolean adminDeleteUser(String userName)
		{
			try
			{
				var collection = dbManager.DataBase.GetCollection<MongoUserModel>("USER");
				var filterBuilder = Builders<MongoUserModel>.Filter;
				var filter = filterBuilder.Eq("userName", userName);
				collection.DeleteOne(filter);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
		}


	}
}
