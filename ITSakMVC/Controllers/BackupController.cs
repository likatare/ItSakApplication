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
    public class BackupController : Controller
    {
        // GET: BackupController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BackupController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: BackupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string input)
        {
            try
            {
                ITSakLibraryApp.CreateBackup(input);

                if (input != null)
                {
                    ViewBag.ConfirmedMessage = "Database saved";
                    return View();

                }

                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // GET: BackupController/Restore
        public ActionResult Restore()
        {

            List<string> backupfiles = ITSakLibraryApp.ListOfFiles();

            return View(backupfiles.ToList());
        }

        // POST: BackupController/Restore/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(string backupFile)
        {
            try
            {
                List<string> backupfiles = ITSakLibraryApp.ListOfFiles();
                if (backupFile != null)
                {
                    ITSakLibraryApp.RestoreBackupMVC(backupFile);
                    ViewBag.ConfirmedMessage = "Database restored";
                    
                    return View(backupfiles.ToList());
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
