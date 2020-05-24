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
				ViewData["currentOpen"] = "EditUser";
				return RedirectToAction("Administration", "Administration");
			}
			else
			{
				ViewData["message"] = "Wrong user name or password!";
			}


			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			TempData["msg"] = "<script>alert('Logout successfully!');</script>";
			return RedirectToAction("Administration", "Administration");
		}


		//updateUserPassword
		public IActionResult UserSetting()//<-------------------------- fake data
		{

			if (HttpContext.Session.Get<MongoUserModel>("user") == null)
			{
				return RedirectToAction("Login", "Administration");
			}
			else
			{
				var userSession = HttpContext.Session.Get<MongoUserModel>("user");
				UserLogic userLogic = new UserLogic();
				var list = userLogic.getPersonalUsagePlan(userSession.userName);
				if (list != null)
				{
					foreach (MongoPersonalUsagePlanModel m in list)
					{
						Debug.WriteLine("m :" + m.userName);
					}
					ViewData["userPlan"] = list;
				}

				var user = userLogic.getUserModel(userSession.userName);
				HttpContext.Session.Set<MongoUserModel>("user", user);
				ViewData["userName"] = user.userName;
				ViewData["fName"] = user.fName;
				ViewData["lName"] = user.lName;
			}

			return View();
		}

		public IActionResult Administration()
		{
			if (HttpContext.Session.Get<MongoUserModel>("user") == null)
			{
				return RedirectToAction("Login", "Administration");
			}
			else{
				UserLogic userLogic = new UserLogic();
				var userList = userLogic.getAllUserModel();
				ViewData["userList"] = userList;
			}

			return View();
		}

		[HttpPost]
		public IActionResult UserSetting(String userName, String currentOpen, double pTemp, double pHum, double pLig, String desc, String name)
		{
			Debug.WriteLine("Here is post");
			UserLogic userLogic = new UserLogic();
			userLogic.updatePersonalUsagePlan(userName,pTemp,pHum,pLig,desc,name);
			ViewData["currentOpen"] = currentOpen;
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpGet]
		public IActionResult AddPlan(String userName, String currentOpen, double pTemp, double pHum, double pLig, String desc, String name)
		{
			Debug.WriteLine("Here is AddPlan");
			UserLogic userLogic = new UserLogic();
			var isCreateSuccess = userLogic.createPersonalUsagePlan(userName, pTemp, pHum, pLig, desc, name);
			if (!isCreateSuccess)
			{
				Debug.WriteLine("Plan already exists");
				TempData["msg"] = "<script>alert('Plan already exists, please change other name');</script>";
			}
			else
			{
				TempData["msg"] = "<script>alert('Plan create successfully');</script>";
			}
			ViewData["currentOpen"] = currentOpen;
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpGet]
		public IActionResult DeletePlan(String userName, String currentOpen, String name)
		{
			Debug.WriteLine("Here is delete Plan");
			UserLogic userLogic = new UserLogic();
			var isDeleteSuccess = userLogic.deletePersonalUsagePlan(userName, name);
			if (isDeleteSuccess)
			{
				Debug.WriteLine("Plan deleted");
				TempData["msg"] = "<script>alert('Plan delete successfully!');</script>";
			}
			ViewData["currentOpen"] = currentOpen;
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpPost]
		public IActionResult UpdatePwd(String userName, String currentOpen ,String password ,String cPassword)
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
				TempData["msg"] = "<script>alert('User password update successfully!');</script>";
			}
			ViewData["currentOpen"] = currentOpen;
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpPost]
		public IActionResult UpdateInfo(String userName, String currentOpen, String fName, String lName)
		{
			Debug.WriteLine("Here is Update Pwd");
			UserLogic userLogic = new UserLogic();
			var isUpdateSuccess = userLogic.updateUserInfo(userName, fName, lName);
			if (isUpdateSuccess)
			{
				Debug.WriteLine("user info is Update Success");
				TempData["msg"] = "<script>alert('User information update successfully!');</script>";
			}
			ViewData["currentOpen"] = currentOpen;
			return RedirectToAction("UserSetting", "Administration");
		}

		[HttpPost]
		public IActionResult AdminAddUser(String userName, String fName, String lName, String password)
		{
			Debug.WriteLine("Here is AdminAddUser");
			UserLogic userLogic = new UserLogic();
			var isCreateSuccess = userLogic.adminAddUser(userName, fName, lName, password);
			if (!isCreateSuccess)
			{
				Debug.WriteLine("User already exists");
				TempData["msg"] = "<script>alert('User already exists, please change other user name');</script>";
			}
			else
			{
				TempData["msg"] = "<script>alert('User create successfully');</script>";
			}
			return RedirectToAction("Administration", "Administration");
		}

		[HttpPost]
		public IActionResult AdminUpdateInfo(String userName, String fName, String lName,String password)
		{
			Debug.WriteLine("Here is Update AdminUpdateInfo");
			UserLogic userLogic = new UserLogic();
			var isUpdateSuccess = userLogic.adminUpdateUserInfo(userName, fName, lName, password);
			if (isUpdateSuccess)
			{
				Debug.WriteLine("user Admin info is Update Success");
				TempData["msg"] = "<script>alert('User information update successfully!');</script>";
			}
			return RedirectToAction("Administration", "Administration");
		}

		[HttpPost]
		public IActionResult AdminDeleteUser(String userName)
		{
			Debug.WriteLine("Here is Update AdminDeleteUser");
			UserLogic userLogic = new UserLogic();
			var isUpdateSuccess = userLogic.adminDeleteUser(userName);
			if (isUpdateSuccess)
			{
				Debug.WriteLine("user delete Success");
				TempData["msg"] = "<script>alert('User deleted!');</script>";
			}
			return RedirectToAction("Administration", "Administration");
		}
	}
}