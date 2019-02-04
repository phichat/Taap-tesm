using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaapApp.Controllers
{
    public class ReportWmsPartReceiptController : Controller
    {
        public string GetMetaForm()
        {
            return "rpt-wms-part-receipt";
        }
        public IActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }
    }
}