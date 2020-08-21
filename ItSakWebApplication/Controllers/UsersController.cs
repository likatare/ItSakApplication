using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using ItSakWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Repository;
using Repository.Models;

namespace ItSakWebApplication.Controllers
{
    public class UsersController : Controller
    {
        // GET: UsersController
        /// <summary>
        /// Gets all members from the database.
        /// </summary>
        /// <returns>A list of memebers</returns>
        public ActionResult Index()
        {
            List<User> users = UserRepository.GetUsers();

            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(string id)
        {
            User user = UserRepository.GetUserById(id);

            return View(user);
        }

        // GET: UsersController/Create
        public ActionResult Create()

        {
            User user = new User();

            return View(user);
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                var hashedPassword = Crypto.HashPassword(user.Password);



                if (UserRepository.GetUserByUsename(user.UserName) == null)
                {

                    UserRepository.CreateUser(user.UserName, hashedPassword, user.Description);
                }
                else
                {
                    user.ErrorMessage = "Username alreaady exists";

                    return View(user);
                }


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private static string GetUserPasswordByUsername(string username)
        {
            User user = UserRepository.GetUserByUsename(username);

            string password = "";

            if (user != null)
            {
                password = user.Password;
            }

            return password;

        }


    }
}
