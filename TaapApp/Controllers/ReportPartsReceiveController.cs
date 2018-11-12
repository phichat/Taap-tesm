using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaapApp.Models;

namespace TaapApp.Controllers
{
    public class ReportPartsReceiveController : Controller
    {
        private readonly db_TaapContext _context;

        public ReportPartsReceiveController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "rpt-parts-receive";
        }

        // GET: ReportPartsReceive
        public IActionResult Index()
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
        public async Task<IActionResult> Search(string receiveNo, DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom == null || dateTo == null)
                return NoContent();

            var p1 = receiveNo;
            var p2 = dateFrom.Date;
            var p3 = dateTo.Date;
            var list = await _context.SPReportPartReceive
                .FromSql("sp_rptPartsReceive @p0, @p1, @p2", 
                parameters: new[] {
                    p1,
                    p2.ToString("yyyy-MM-dd"),
                    p3.ToString("yyyy-MM-dd")
                }).ToListAsync();

            if (list == null)
                return NoContent();

            return Json(list);
        }

    }
}
