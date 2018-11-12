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
    public class ReportPartsMovementController : Controller
    {
        private readonly db_TaapContext _context;

        public ReportPartsMovementController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "rpt-parts-movement";
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

            ViewBag.SelectOptionPartsNumber = (from p in _context.PartReceive
                                               orderby p.ReceiveNo descending
                                               select new SelectOption
                                               {
                                                   Value = p.PartNo,
                                                   Text = p.PartNo
                                               }).Distinct().ToList();

            var list = new List<PartReceive>();

            ViewBag.MetaForm = GetMetaForm();

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string model, string partNo, DateTime dateFrom, DateTime dateTo)
        {
            if (model == null || partNo == null || dateFrom == null || dateTo == null)
                return NoContent();

            var p0 = model;
            var p1 = partNo;
            var p2 = dateFrom.Date;
            var p3 = dateTo.Date;

            var list = new List<ReportPartMovement>();

            try
            {
                list = await _context.SPReportPartMovement
               .FromSql("sp_rptPartsMovement @p0, @p1, @p2, @p3",
               parameters: new[] {
                    p0,
                    p1,
                    p2.ToString("yyyy-MM-dd"),
                    p3.ToString("yyyy-MM-dd")
               }).ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


            return Json(list);
        }
    }
}