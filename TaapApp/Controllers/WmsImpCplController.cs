using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class WmsImpCplController : Controller
    {

        public string GetMetaForm()
        {
            return "wms-imp-cpl";
        }

        // GET: WmsImpCpl
        public IActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        // GET: WmsImpCpl/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WmsImpCpl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WmsImpCpl/Create
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

        // GET: WmsImpCpl/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WmsImpCpl/Edit/5
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

        // GET: WmsImpCpl/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WmsImpCpl/Delete/5
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