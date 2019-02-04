using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class WmsTFMoveController : Controller
    {
        public string GetMetaForm()
        {
            return "wms-tf-move";
        }

        // GET: WmsTFMove
        public ActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        // GET: WmsTFMove/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WmsTFMove/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WmsTFMove/Create
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

        // GET: WmsTFMove/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WmsTFMove/Edit/5
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

        // GET: WmsTFMove/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WmsTFMove/Delete/5
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