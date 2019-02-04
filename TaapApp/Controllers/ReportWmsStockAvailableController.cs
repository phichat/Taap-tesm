using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class ReportWmsStockAvailableController : Controller
    {
        public string GetMetaForm()
        {
            return "rpt-wms-stock-available";
        }
        public IActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }
    }
}