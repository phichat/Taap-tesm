using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class ReportWmsPartMovementController : Controller
    {
        public string GetMetaForm()
        {
            return "rpt-wms-part-movement";
        }
        public IActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }
    }
}