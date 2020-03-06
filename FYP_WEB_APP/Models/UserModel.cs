using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace FYP_WEB_APP.Models
{
	public class UserModel
	{
		public string userId { get; set; }
		public string name { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
		public string role { get; set; }// for future use

		public UserModel()
		{

			userId = "Null id";
			name = "Null name";
			userName = "Null";
			password = "123";
			role = "Null id";

		}

		public UserModel(string name,string username,string password,string role)
		{
			this.userId = "";
			this.name = name;
			this.userName = username;
			this.password = password;
			this.role = role;
		}
	}

	
}
