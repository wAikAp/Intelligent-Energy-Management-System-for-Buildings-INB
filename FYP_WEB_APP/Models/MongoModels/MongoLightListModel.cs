using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace FYP_WEB_APP.Models.MongoModels
{
	[System.Runtime.InteropServices.Guid("95060E51-338C-4A71-B6C7-5190D4B455B2")]
	public class MongoLightListModel
	{
		public ObjectId _id { get; set; }
		public string roomId { get; set; }
		public string lightId { get; set; }
		public string location { get; set; }
		public string desc { get; set; }
		public BsonDateTime lastest_checking_time { get; set; }// for future use
		public double total_run_time { get; set; }// for future use
		


	}
}
