using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISSF2020.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Libmongocrypt;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace ISSF2020.Pages
{
    public class LoginModel : PageModel
    {

        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Success { get; set; }
        [TempData]
        public string Error { get; set; }

        private readonly UserService _userService;
        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {

            var form = HttpContext.Request.Form;

            var user = _userService.Get(form["username"]);
            if (user is null)
            {
                Console.WriteLine($"User {form["username"]} doesn't exist.");
                Error = $"Entered username and password do not match.";
                return RedirectToPage("/account/login");
            }
            // if user exists, check if form password == user password

            var validPassword = _userService.CheckPass(form["username"], form["password"]);
            if (validPassword == true)
            {
                Console.WriteLine($"User {form["username"]} - valid pass");
                Success = $"Successfully logged in as {user.Username}.";
                HttpContext.Session.SetString("User", user.Username);
                
                return RedirectToPage("/Index");
            } 
            else
            {
                Console.WriteLine($"User {form["username"]} - invalid pass");
                Error = $"Entered username and password do not match.";
                return RedirectToPage("/account/login");
            }
        }
    }
}
