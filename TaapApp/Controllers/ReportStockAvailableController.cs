using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaapApp.Models;

namespace TaapApp.Controllers
{
    public class ReportStockAvailableController : Controller
    {
        private readonly db_TaapContext _context;

        public ReportStockAvailableController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "rpt-stock-available";
        }


        // GET: ReportStockAvailable
        public ActionResult Index()
        {
            ViewBag.SelectOptionReceiveNo = (from p in _context.PartReceive
                                             orderby p.ReceiveNo descending
                                             select new SelectOptionReceiveNo
                                             {
                                                 Value = p.ReceiveNo,
                                                 Text = p.ReceiveNo
                                             }).Distinct().ToList();

            var list = new List<PartReceive>();

            ViewBag.MetaForm = GetMetaForm();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string receiveNo)
        {
            if (receiveNo == null)
                return NoContent();

            var list = await _context.SPReportStorcAvailable
                .FromSql("sp_rptStockAvailable @p0",
               parameters: new[] {
                    receiveNo
                }).ToListAsync();

            if (list == null)
                return NoContent();

            return Json(list);
        }
    }
}