using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISSF2020.Pages.Account
{
    public class LogoutModel : PageModel
    {

        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Success { get; set; }
        [TempData]
        public string Error { get; set; }

        public void OnGet()
        {
            ViewData["User"] = HttpContext.Session.GetString("User");
        }
        public IActionResult OnPost()
        {
            var form = HttpContext.Request.Form;
            var action = form["action"];

            if (action == "accept")
            {
                HttpContext.Session.Clear();
                Message = "Logged out, session cleared.";
                return RedirectToPage("/index");
            }
            else if (action == "cancel")
            {
                Console.WriteLine("logout cancelled");
                return RedirectToPage("/index");
            }
            else
            {
                Console.Write("logout submit button has neither accept nor cancel as value");
                Message = "Logout button value invalid?";
                return RedirectToPage("/account/logout");
            }
        }
    }
}
 