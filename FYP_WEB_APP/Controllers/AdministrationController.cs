using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP_APP.Extensions;
using FYP_APP.Models.LogicModels;
using FYP_APP.Models.MongoModels;
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
				return RedirectToAction("Home", "Home");
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
				return RedirectToAction("Dashboard", "Home");
			}
			else
			{
				ViewData["message"] = "Wrong user name or password!";

			}


			return View();
		}


		public IActionResult UserSetting()
		{
			if (HttpContext.Session.Get<MongoUserModel>("user") != null)
			{
				return RedirectToAction("Home", "Home");
			}

			return View();
		}
	}
}