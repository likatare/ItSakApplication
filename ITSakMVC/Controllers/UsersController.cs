using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryITSak;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Models;

namespace ITSakMVC.Controllers
{
    public class UsersController : Controller
    {
        // GET: UsersController
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
                var hashPassword = ITSakLibraryApp.HashPassword(user.Password);

                if (UserRepository.GetUserByUsername(user.UserName) == null)
                {
                    UserRepository.CreateUser(user.UserName,hashPassword,user.Description);
                }
                else
                {
                    ViewBag.Errormessage = "Username already exists";
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
        public ActionResult Edit(string id)
        {
            User user = UserRepository.GetUserById(id);

            return View(user);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,User user)
        {
            try
            {
                UserRepository.EditUserById(id,user);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(string id)
        {
            User user = UserRepository.GetUserById(id);


            return View(user);
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                UserRepository.DeleteUserById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
