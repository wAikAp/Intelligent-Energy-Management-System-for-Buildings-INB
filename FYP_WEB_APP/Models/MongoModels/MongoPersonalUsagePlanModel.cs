using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_WEB_APP.Models.MongoModels
{
	public class MongoPersonalUsagePlanModel
	{
		public ObjectId _id { get; set; }
		public string userName { get; set; }
		public double pTemp { get; set; }
		public double pHum { get; set; }
		public double pLig { get; set; }
		public String desc { get; set; }
		public String name { get; set; }
	}
}
