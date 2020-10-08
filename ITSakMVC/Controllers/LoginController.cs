using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryITSak;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;

namespace ITSakMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult Index()
        {
            User user = new User();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            try
            {
                string hashedPassword = ITSakLibraryApp.GetUserByUsername(user.Username);

                if (hashedPassword.Length > 0)
                {

                    string[] splittedInput = hashedPassword.Split(':');
                    string salt = splittedInput[1];

                    string combinedPasswordSalt = $"{user.Password}:{salt}";
                    string hashedResult = ITSakLibraryApp.CreateMd5(combinedPasswordSalt);

                    if (hashedResult == splittedInput[0])
                    {
                        ViewBag.MatchingUsername = "It´s a match";
                        return View(user);
                    }
                    else
                    {
                        ViewBag.MatchingUsername = "It doesn`t match";
                        return View(user);
                    }
                }

                else
                {
                    ViewBag.MatchingUsername = "User doesn't exist";
                    return View(user);

                }
                
            }
            catch
            {
                return View();
            }
        }
    }
}
