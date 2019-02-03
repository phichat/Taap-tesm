using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class WmsImpFplController : Controller
    {
        public string GetMetaForm()
        {
            return "wms-imp-fpl";
        }

        // GET: WmsImpFpl
        public IActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        // GET: WmsImpFpl/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WmsImpFpl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WmsImpFpl/Create
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

        // GET: WmsImpFpl/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WmsImpFpl/Edit/5
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

        // GET: WmsImpFpl/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WmsImpFpl/Delete/5
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