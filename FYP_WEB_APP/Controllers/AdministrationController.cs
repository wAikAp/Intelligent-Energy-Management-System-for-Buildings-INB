using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FYP_APP.Extensions;
using FYP_APP.Models.LogicModels;
using FYP_APP.Models.MongoModels;
using FYP_WEB_APP.Models.MongoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FYP_WEB_APP.Controllers
{
    public class AdministrationController : Controller
    {
		public IActionResult Login()
		{
			if (HttpContext.Session.Get<MongoUserModel>("user") != null)
			{
				return RedirectToAction("Administration", "Administration");
			}

			return View();
		}

		[HttpPost]
		public IActionResult Login(string loginUserName, string loginPass)
		{

			UserLogic userLogic = new UserLogic();
			MongoUserModel user = userLogic.login(loginUserName, loginPass);

			if (user != null)
			{
				HttpContext.Session.Set<MongoUserModel>("user", user);
				HttpContext.Session.SetString("userName", user.lName);
				//Debug.WriteLine("json " + HttpContext.Session.Get<MongoUserModel>("user"));
				return RedirectToAction("Administration", "Administration");
			}
			else
			{
				ViewData["message"] = "Wrong user name or password!";
			}


			return View();
		}

		//updateUserPassword
		public IActionResult UserSetting()//<-------------------------- fake data
		{
			HttpContext.Session.SetString("userName","WongMY");

			UserLogic userLogic = new UserLogic();
			var list = userLogic.getPersonalUsagePlan("WongMY");
			if(list != null)
			{
				foreach(MongoPersonalUsagePlanModel m in list)
				{
					Debug.WriteLine("m :" + m.userName);
				}
				ViewData["userPlan"] = list;
			}

			var user = userLogic.getUserModel("WongMY");
			ViewData["userName"] = user.userName;
			ViewData["fName"] = user.fName;
			ViewData["lName"] = user.lName;
			return View();
		}

		public IActionResult Administration()
		{
			if (HttpContext.Session.Get<MongoUserModel>("user") == null)
			{
				return RedirectToAction("Login", "Administration");
			}
			else{

			}

			return View();
		}
		[HttpPost]
		public IActionResult UserSetting(String userName, double pTemp, double pHum, double pLig, String desc, String name)
		{
			Debug.WriteLine("Here is post");
			UserLogic userLogic = new UserLogic();
			userLogic.updatePersonalUsagePlan(userName,pTemp,pHum,pLig,desc,name);
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpGet]
		public IActionResult AddPlan(String userName, double pTemp, double pHum, double pLig, String desc, String name)
		{
			Debug.WriteLine("Here is AddPlan");
			UserLogic userLogic = new UserLogic();
			var isCreateSuccess = userLogic.createPersonalUsagePlan(userName, pTemp, pHum, pLig, desc, name);
			if (!isCreateSuccess)
			{
				Debug.WriteLine("Plan already exists");
				TempData["msg"] = "<script>alert('Plan already exists, please change other name');</script>";
			}
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpGet]
		public IActionResult DeletePlan(String userName, String name)
		{
			Debug.WriteLine("Here is delete Plan");
			UserLogic userLogic = new UserLogic();
			var isDeleteSuccess = userLogic.deletePersonalUsagePlan(userName, name);
			if (isDeleteSuccess)
			{
				Debug.WriteLine("Plan deleted");
				TempData["msg"] = "<script>alert('Plan delete successfully!');</script>";
			}
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpPost]
		public IActionResult UpdatePwd(String userName,String password ,String cPassword)
		{
			Debug.WriteLine("Here is Update Pwd");
			UserLogic userLogic = new UserLogic();
			if(password != cPassword)
			{
				TempData["msg"] = "<script>alert('New password not match!');</script>";
			}
			else
			{
				userLogic.updateUserPassword(userName, password);
				Debug.WriteLine("updateUserPassword ed");
			}			
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpPost]
		public IActionResult UpdateInfo(String userName, String fName, String lName)
		{
			Debug.WriteLine("Here is Update Pwd");
			UserLogic userLogic = new UserLogic();
			var isUpdateSuccess = userLogic.updateUserInfo(userName, fName, lName);
			if (isUpdateSuccess)
			{
				Debug.WriteLine("user info is Update Success");
				//TempData["msg"] = "<script>alert('User information update successfully!');</script>";
			}
			return RedirectToAction("UserSetting", "Administration");
		}
	}
}