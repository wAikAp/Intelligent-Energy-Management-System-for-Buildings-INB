using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FYP_APP.Models.MongoModels
{
	public class MongoUserModel
	{
		public ObjectId _id { get; set; }
		public string userId { get; set; }
		public string lName { get; set; }
		public string fName { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
		public string role { get; set; }// for future use
	}
}
