using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class WmsRptMovementController : Controller
    {
        // GET: WmsRptMovement
        public ActionResult Index()
        {
            return View();
        }

        // GET: WmsRptMovement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WmsRptMovement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WmsRptMovement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WmsRptMovement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WmsRptMovement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WmsRptMovement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WmsRptMovement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}