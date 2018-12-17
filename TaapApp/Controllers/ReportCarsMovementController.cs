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


        [HttpGet, ActionName("ExportCPL")]
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
                    var _C01 = new List<string>();
                    for (int i = 0; i < 25; i++) { _C01.Add("A"); }
                    var C01 = string.Join("", _C01);

                    var C02 = $"{x.DateFG.ToString("yyyy")}{x.DateFG.ToString("MM")}{x.DateFG.ToString("dd")}";

                    var _C03 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C03.Add("A"); }
                    var C03 = string.Join("", _C03);

                    var _C04 = new List<int>();
                    for (int i = 0; i < 3; i++) { _C04.Add(1 + i); }
                    var C04 = string.Join("", _C04);

                    var _C05 = new List<string>();
                    for (int i = 0; i < 14; i++) { _C05.Add("A"); }
                    var C05 = string.Join("", _C05);

                    var _C06 = new List<int>();
                    for (int i = 0; i < 9; i++) { _C06.Add(1 + i); }
                    var C06 = string.Join("", _C06);

                    var _C07 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C07.Add("A"); }
                    var C07 = string.Join("", _C07);

                    var _C08 = new List<string>();
                    for (int i = 0; i < 17; i++) { _C08.Add("A"); }
                    var C08 = string.Join("", _C08);

                    // Length 30
                    var _C09 = new List<string> { "THAILAND" }; 
                    for (int i = 0; i < 22; i++) { _C09.Add("A"); }
                    var C09 = string.Join("", _C09);

                    var _C10 = new List<string>();
                    for (int i = 0; i < 2; i++) { _C10.Add("A"); }
                    var C10 = string.Join("", _C10);

                    var _C11 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C11.Add("9"); }
                    var C11 = string.Join("", _C11);

                    var C12 = "SEAL000001";

                    var C13 = x.SetNo;

                    var C14 = x.PackingMonth;

                    var _C15 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C15.Add("A"); }
                    var C15 = string.Join("", _C15);

                    var C16 = "999,999";

                    var C17 = C16;

                    var C18 = "9999999,999";

                    var C19 = C18;

                    var C20 = 0;

                    var C21 = C20;

                    var C22 = "A";

                    var C23 = x.CommissionFrom;

                    var C24 = x.CommissionTo;

                    var _C25 = new List<string>();
                    for (int i = 0; i < 30; i++) { _C25.Add("A"); }
                    var C25 = string.Join("", _C25);

                    var _C26 = new List<string>{ x.Model };
                    var _C26Length = 30 - x.Model.Length;
                    for (int i = 0; i < _C26Length; i++) { _C26.Add("A"); }
                    var C26 = string.Join("", _C26);

                    var C27 = "0051";

                    var C28 = "1495";

                    var C29 = "1140";

                    var C30 = "0850";

                    var _C31 = new List<string>();
                    for (int i = 0; i < 15; i++) { _C31.Add("A"); }
                    var C31 = string.Join("", _C31);

                    var _C32 = new List<string>();
                    for (int i = 0; i < 72; i++) { _C32.Add("A"); }
                    var C32 = string.Join("", _C32);

                    text.AppendLine($"{C01}{C02}{C03}{C04}{C05}{C06}{C07}" +
                                    $"{C08}{C09}{C10}{C11}{C12}{C13}{C14}" +
                                    $"{C15}{C16}{C17}{C18}{C19}{C20}{C21}" +
                                    $"{C22}{C23}{C24}{C25}{C26}{C27}{C28}" +
                                    $"{C29}{C30}{C31}{C32}");
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
            //memory.Position = 0;

            return File(memory, GetContentType(path), Path.GetFileName(path));

        }

        [HttpGet, ActionName("ExportFPL")]
        public async Task<IActionResult> ExportFPL(string model, string packingMonth, DateTime dateFrom, DateTime dateTo) {
            if (model == null || packingMonth == null)
                return NoContent();

            string sWebRootFolder = _hostingEnv.WebRootPath;
            var path = Path.Combine(sWebRootFolder, @"uploads\FPL.txt");
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

                var list = await _context.SPReportFpl
                    .FromSql("sp_rptFPL @p0, @p1, @p2, @p3",
                    parameters: new[] {
                    p0,
                    p1,
                    p2.ToString("yyyy-MM-dd"),
                    p3.ToString("yyyy-MM-dd")
                }).ToListAsync();

                int j = 1;

                list.ForEach(x =>
                {
                    var C01 = "010";

                    var C02 = "001";

                    var C03 = "0500";

                    var C04 = x.Year;

                    var C05 = x.Month;

                    var C06 = j.ToString().PadLeft(4, '0');

                    var _C07 = new List<int>();
                    for (int i = 0; i < 3; i++) { _C07.Add(1 + i); }
                    var C07 = string.Join("", _C07);

                    // Length 30
                    var _C08 = new List<string> { "THAILAND" };
                    for (int i = 0; i < 22; i++) { _C08.Add("A"); }
                    var C08 = string.Join("", _C08);

                    var _C09 = new List<int>();
                    for (int i = 0; i < 8; i++) { _C09.Add(1 + i); }
                    var C09 = string.Join("", _C09);

                    var _C10 = new List<string> { x.Model };
                    var _C10Length = 30 - x.Model.Length;
                    for (int i = 0; i < _C10Length; i++) { _C10.Add("A"); }
                    var C10 = string.Join("", _C10);

                    var _C11 = new List<string>();
                    for (int i = 0; i < 7; i++) { _C11.Add("A"); }
                    var C11 = string.Join("", _C11);

                    var _C12 = new List<string>();
                    for (int i = 0; i < 7; i++) { _C12.Add("A"); }
                    var C12 = string.Join("", _C12);

                    var _C13 = new List<string>();
                    for (int i = 0; i < 7; i++) { _C13.Add("A"); }
                    var C13 = string.Join("", _C13);

                    var _C14 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C14.Add("A"); }
                    var C14 = string.Join("", _C14);

                    var _C15 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C15.Add("A"); }
                    var C15 = string.Join("", _C15);

                    var _C16 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C16.Add("A"); }
                    var C16 = string.Join("", _C16);

                    var C17 = $"{DateTime.Now.ToString("dd")}{DateTime.Now.ToString("MM")}{DateTime.Now.ToString("yyyy")}";

                    var C18 = j.ToString().PadLeft(2, '0');

                    var _C19 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C19.Add("A"); }
                    var C19 = string.Join("", _C19);

                    var _C20 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C20.Add("A"); }
                    var C20 = string.Join("", _C20);

                    var _C21 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C21.Add("A"); }
                    var C21 = string.Join("", _C21);

                    var C22 = "H";

                    var C23 = j.ToString().PadLeft(2, '0');

                    var C24 = "A";

                    var C25 = j.ToString().PadLeft(12, '0');

                    var C26 = "A";

                    var _C27 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C27.Add("A"); }
                    var C27 = string.Join("", _C27);

                    var _C28 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C28.Add("A"); }
                    var C28 = string.Join("", _C28);

                    var C29 = j.ToString().PadLeft(5, '0'); ;

                    var _C30 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C30.Add("A"); }
                    var C30 = string.Join("", _C30);

                    var _C31 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C31.Add("A"); }
                    var C31 = string.Join("", _C31);

                    var C32 = "+";

                    var C33 = "999999,999";

                    var C34 = "01";

                    var _C35 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C35.Add("A"); }
                    var C35 = string.Join("", _C35);

                    var _C36 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C36.Add("A"); }
                    var C36 = string.Join("", _C36);

                    var C37 = j.ToString().PadLeft(6, '0');

                    var C38 = "9999,999";

                    var C39 = "999,99";

                    var C40 = "M";

                    var C41 = "+";

                    var C42 = "999999,999";

                    var _C43 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C43.Add("A"); }
                    var C43 = string.Join("", _C43);

                    var _C44 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C44.Add("A"); }
                    var C44 = string.Join("", _C44);

                    var _C45 = new List<string>();
                    for (int i = 0; i < 30; i++) { _C45.Add("A"); }
                    var C45 = string.Join("", _C45);

                    var _C46 = new List<string>();
                    for (int i = 0; i < 30; i++) { _C46.Add("A"); }
                    var C46 = string.Join("", _C46);

                    var _C47 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C47.Add("A"); }
                    var C47 = string.Join("", _C47);

                    var _C48 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C48.Add("A"); }
                    var C48 = string.Join("", _C48);

                    var _C49 = new List<int>();
                    for (int i = 0; i < 7; i++) { _C49.Add(1 + i); }
                    var C49 = string.Join("", _C49);

                    var C50 = j.ToString().PadLeft(5, '0');

                    var C60 = j.ToString().PadLeft(6, '0');

                    var C61 = j.ToString().PadLeft(4, '0');

                    var C62 = j.ToString().PadLeft(4, '0');

                    var _C63 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C63.Add("A"); }
                    var C63 = string.Join("", _C63);

                    var _C64 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C64.Add("A"); }
                    var C64 = string.Join("", _C64);

                    var _C65 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C65.Add("A"); }
                    var C65 = string.Join("", _C65);

                    var _C66 = new List<string>();
                    for (int i = 0; i < 150; i++) { _C66.Add("A"); }
                    var C66 = string.Join("", _C66);

                    var _C67 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C67.Add("A"); }
                    var C67 = string.Join("", _C67);

                    var _C68 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C68.Add("A"); }
                    var C68 = string.Join("", _C68);

                    var _C69 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C69.Add("A"); }
                    var C69 = string.Join("", _C69);

                    var C70 = "+";

                    var C71 = "99999,99";

                    var C72 = "+";

                    var C73 = "999999,999";

                    var C74 = x.SetNo;

                    var C75 = "+";

                    var C76 = "999,99999";

                    var _C77 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C77.Add("A"); }
                    var C77 = string.Join("", _C77);

                    var C78 = "99999,99";

                    var C79 = "999999,99";

                    var _C80 = new List<string>();
                    for (int i = 0; i < 10; i++) { _C80.Add("A"); }
                    var C80 = string.Join("", _C80);

                    var _C81 = new List<string>();
                    for (int i = 0; i < 16; i++) { _C81.Add("A"); }
                    var C81 = string.Join("", _C81);

                    var _C82 = new List<string>();
                    for (int i = 0; i < 13; i++) { _C82.Add("A"); }
                    var C82 = string.Join("", _C82);

                    var C83 = "AA";

                    var _C84 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C84.Add("A"); }
                    var C84 = string.Join("", _C84);

                    var C85 = "J";

                    var _C86 = new List<string>();
                    for (int i = 0; i < 18; i++) { _C86.Add("A"); }
                    var C86 = string.Join("", _C86);

                    var _C87 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C87.Add("A"); }
                    var C87 = string.Join("", _C87);

                    var C88 = "999999,999";

                    var C89 = "999999,99999999";

                    var C90 = "999999,99";

                    var C91 = "99999999,99";

                    var _C92 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C92.Add("A"); }
                    var C92 = string.Join("", _C92);

                    j++;

                    text.AppendLine($"{C01}{C02}{C03}{C04}{C05}{C06}{C07}" +
                                    $"{C08}{C09}{C10}{C11}{C12}{C13}{C14}" +
                                    $"{C15}{C16}{C17}{C18}{C19}{C20}{C21}" +
                                    $"{C22}{C23}{C24}{C25}{C26}{C27}{C28}" +
                                    $"{C29}{C30}{C31}{C32}{C33}{C34}{C35}" + 
                                    $"{C36}{C37}{C38}{C39}{C40}{C41}{C42}" +
                                    $"{C43}{C44}{C45}{C46}{C47}{C48}{C49}" +
                                    $"{C50}{C60}{C61}{C62}{C63}{C64}{C65}" +
                                    $"{C66}{C67}{C68}{C69}{C70}{C71}{C72}" +
                                    $"{C73}{C74}{C75}{C76}{C77}{C78}{C79}" +
                                    $"{C80}{C81}{C82}{C83}{C84}{C85}{C86}" +
                                    $"{C87}{C88}{C89}{C90}{C91}{C92}");
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
            //memory.Position = 0;

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