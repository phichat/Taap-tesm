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
    public class ReportCarsMovementController : Controller
    {
        private readonly db_TaapContext _context;

        public ReportCarsMovementController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "rpt-cars-movement";
        }

        // GET: ReportStockAvailable
        public ActionResult Index()
        {
            ViewBag.SelectOptionModel = (from p in _context.PartReceive
                                         orderby p.ReceiveNo descending
                                         select new SelectOption
                                         {
                                             Value = p.Model,
                                             Text = p.Model
                                         }).Distinct().ToList();

            ViewBag.SelectOptionPM = (from p in _context.PartReceive
                                      orderby p.ReceiveNo descending
                                      select new SelectOption
                                      {
                                          Value = p.PackingMonth,
                                          Text = p.PackingMonth
                                      }).Distinct().ToList();

            var list = new List<PartReceive>();

            ViewBag.MetaForm = GetMetaForm();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string model, string packingMonth, DateTime dateFrom, DateTime dateTo)
        {
            if (model == null || packingMonth == null || dateFrom == null || dateTo == null)
                return NoContent();

            var p0 = model;
            var p1 = packingMonth;
            var p2 = dateFrom.Date;
            var p3 = dateTo.Date;

            var list = await _context.SPReportCarMovement
                .FromSql("sp_rptCarsMovement @p0, @p1, @p2, @p3",
                parameters: new[] {
                    p0,
                    p1,
                    p2.ToString("yyyy-MM-dd"),
                    p3.ToString("yyyy-MM-dd")
                }).ToListAsync();

            if (list == null)
                return NoContent();

            return Ok(list);
        }
    }
}