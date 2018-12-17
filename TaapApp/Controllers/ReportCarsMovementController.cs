using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaapApp.Models;

namespace TaapApp.Controllers
{
    public class ReportCarsMovementController : Controller
    {
        private readonly db_TaapContext _context;
        private IHostingEnvironment _hostingEnv;

        public ReportCarsMovementController(db_TaapContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnv = hostingEnvironment;
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


        [HttpGet("ExportCPL")]
        public async Task<IActionResult> ExportCPL(string model, string packingMonth, DateTime dateFrom, DateTime dateTo)
        {
            if (model == null || packingMonth == null)
                return NoContent();

            string sWebRootFolder = _hostingEnv.WebRootPath;
            var path = Path.Combine(sWebRootFolder, @"uploads\CPL.txt");
            FileInfo fi = new FileInfo(path);
            // Delete the file if it exists.
            if (fi.Exists) fi.Delete();

            //Create the file.
            using (FileStream fs = fi.Create())
            {
                StringBuilder text = new StringBuilder();

                var p0 = model;
                var p1 = packingMonth;
                var p2 = dateFrom.Date;
                var p3 = dateTo.Date;

                var list = await _context.SPReportCpl
                    .FromSql("sp_rptCPL @p0, @p1, @p2, @p3",
                    parameters: new[] {
                    p0,
                    p1,
                    p2.ToString("yyyy-MM-dd"),
                    p3.ToString("yyyy-MM-dd")
                }).ToListAsync();


                list.ForEach(x =>
                {
                    var SHIPNAME = new List<string>();
                    for (int i = 0; i < 25; i++) { SHIPNAME.Add("A"); }
                    var _SHIPNAME = string.Join("", SHIPNAME);

                    var SHIPDEPARTURE = $"{x.DateFG.ToString("yyyy")}{x.DateFG.ToString("MM")}{x.DateFG.ToString("dd")}";

                    var CONSIGNEENAME = new List<string>();
                    for (int i = 0; i < 20; i++) { SHIPNAME.Add("A"); }
                    var _CONSIGNEENAME = string.Join("", CONSIGNEENAME);

                    text.AppendLine($"{_SHIPNAME}{SHIPDEPARTURE}{_CONSIGNEENAME}");

                });

                Byte[] info = new ASCIIEncoding().GetBytes(text.ToString());

                //Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}