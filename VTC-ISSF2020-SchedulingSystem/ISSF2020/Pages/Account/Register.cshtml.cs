using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ISSF2020.Models;
using ISSF2020.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ISSF2020.Pages
{
    public class RegisterModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Success { get; set; }
        [TempData]
        public string Error { get; set; }

        private readonly UserService _userService;
        public RegisterModel(UserService userService) 
        {
            _userService = userService;
        }

        public void OnGet()
        {
            
        }
        public IActionResult OnPost()
        {
            
            var form = HttpContext.Request.Form;

            /*
            for (int i = 0; i < form.Count; i++)
            {
                var key = form.Keys.ElementAt(i);
                var value = form[key];
                Console.WriteLine(value);
            }
            */

            var user = new UserModel
            {
                Username = form["username"],
                Email = form["email"],
                Password = form["password"] // TODO: Encrypt passwords https://www.nuget.org/packages/Konscious.Security.Cryptography.Argon2/
            };

            var existingUser = _userService.Get(form["username"]);
            if (existingUser is null)
            {
                _userService.Create(user);
                Console.WriteLine($"User {user.Username} has been created.");
                Success = $"User {user.Username} has been created.";
                return RedirectToPage("/account/login");

            }
            else
            {
                Console.WriteLine($"User {user.Username} already exists.");
                Error = $"User {user.Username} already exists.";
                ModelState.AddModelError("Username", "Username already exists.");
                return RedirectToPage("/account/register");
            }
        }
    }
}
