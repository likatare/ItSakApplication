using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;

namespace ITSakMVC.Controllers
{
    public class TextController : Controller
    {
        

        // GET: TextController/Create
        public ActionResult Create()
        {
            User user = new User();

            return View(user);
        }

        // POST: TextController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string description )
        {
            try
            {
                User user = new User();

                if (description !=null)
                {
                    user.Description = description;

                    return View(user);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
