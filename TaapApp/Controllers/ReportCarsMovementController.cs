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
                var text = new StringBuilder();

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
                    for (int i = 0; i < 25; i++) { _C01.Add("0"); }
                    var C01 = string.Join("", _C01);

                    var C02 = $"{x.DateFG.ToString("yyyy")}{x.DateFG.ToString("MM")}{x.DateFG.ToString("dd")}";

                    var _C03 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C03.Add("0"); }
                    var C03 = string.Join("", _C03);

                    var _C04 = new List<int>();
                    for (int i = 0; i < 3; i++) { _C04.Add(1 + i); }
                    var C04 = string.Join("", _C04);

                    var CNumber = "CONTEST001";
                    var _C05 = new List<string>{ CNumber };
                    var _C05Length = 14 - CNumber.Length;
                    for (int i = 0; i < _C05Length; i++) { _C05.Add(" "); }
                    var C05 = string.Join("", _C05);

                    // 9
                    var _C06 = new List<string> { "1"};
                    for (int i = 0; i < 8; i++) { _C06.Add(" "); }
                    var C06 = string.Join("", _C06);

                    //var _C07 = new List<string>();
                    //for (int i = 0; i < 6; i++) { _C07.Add("0"); }
                    //var C07 = string.Join("", _C07);
                    var C07 = "000001";

                    var _C08 = new List<string>();
                    for (int i = 0; i < 17; i++) { _C08.Add("0"); }
                    var C08 = string.Join("", _C08);

                    var Country = "001879THAILAND";
                    var _C09 = new List<string> { Country };
                    var _C09Length = 30 - Country.Length;
                    for (int i = 0; i < _C09Length; i++) { _C09.Add(" "); }
                    var C09 = string.Join("", _C09);

                    var _C10 = new List<string>();
                    for (int i = 0; i < 2; i++) { _C10.Add("0"); }
                    var C10 = string.Join("", _C10);

                    var _C11 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C11.Add("0"); }
                    var C11 = string.Join("", _C11);

                    var C12 = "SEAL000001";

                    var C13 = x.SetNo;

                    var C14 = x.PackingMonth;

                    var _C15 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C15.Add("0"); }
                    var C15 = string.Join("", _C15);

                    var C16 = "999,999";

                    var C17 = C16;

                    var C18 = "9999999,999";

                    var C19 = C18;

                    var C20 = 0;

                    var C21 = C20;

                    var C22 = "1";

                    var C23 = x.CommissionFrom;

                    var C24 = x.CommissionTo;

                    var _C25 = new List<string>();
                    for (int i = 0; i < 30; i++) { _C25.Add("0"); }
                    var C25 = string.Join("", _C25);

                    var _C26 = new List<string>{ x.Model };
                    var _C26Length = 30 - x.Model.Length;
                    for (int i = 0; i < _C26Length; i++) { _C26.Add(" "); }
                    var C26 = string.Join("", _C26);

                    var C27 = "02";

                    var C28 = "1495";

                    var C29 = "1140";

                    var C30 = "0850";

                    var variableC31 = $"{C06.Trim()}{C07}";
                    var _C31 = new List<string>();
                    var _C31Length = 15 - variableC31.Length;
                    for (int i = 0; i < _C31Length; i++) { _C31.Add("0"); }
                    var C31 = string.Join("", _C31);

                    var _C32 = new List<string>();
                    for (int i = 0; i < 72; i++) { _C32.Add("0"); }
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
            memory.Position = 0;

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
                    var C01 = "010"; //1

                    var C02 = "001"; //4

                    var C03 = "0500"; //7

                    var C04 = x.Year; //11

                    var C05 = x.Month; //15

                    var C06 = j.ToString().PadLeft(4, '0'); //17

                    var _C07 = new List<int>(); //21
                    for (int i = 0; i < 3; i++) { _C07.Add(1 + i); }
                    var C07 = string.Join("", _C07);

                    // Length 30
                    var _C08 = new List<string> { "THAILAND" }; //24
                    for (int i = 0; i < 22; i++) { _C08.Add("A"); }
                    var C08 = string.Join("", _C08);

                    var _C09 = new List<int>(); //54
                    for (int i = 0; i < 8; i++) { _C09.Add(1 + i); }
                    var C09 = string.Join("", _C09);

                    var _C10 = new List<string> { x.Model }; //62
                    var _C10Length = 30 - x.Model.Length;
                    for (int i = 0; i < _C10Length; i++) { _C10.Add("A"); }
                    var C10 = string.Join("", _C10);

                    var _C11 = new List<int>(); //
                    for (int i = 0; i < 7; i++) { _C11.Add(1 + i); }
                    var C11 = string.Join("", _C11);

                    var _C12 = new List<int>();
                    for (int i = 0; i < 7; i++) { _C12.Add(1 + i); }
                    var C12 = string.Join("", _C12);

                    var _C13 = new List<int>();
                    for (int i = 0; i < 4; i++) { _C13.Add(1 + i); }
                    var C13 = string.Join("", _C13);

                    var _C14 = new List<int>();
                    for (int i = 0; i < 4; i++) { _C14.Add(1 + i); }
                    var C14 = string.Join("", _C14);

                    var _C15 = new List<int>();
                    for (int i = 0; i < 3; i++) { _C15.Add(1 + i); }
                    var C15 = string.Join("", _C15);

                    var C16 = $"{DateTime.Now.ToString("dd")}{DateTime.Now.ToString("MM")}{DateTime.Now.ToString("yyyy")}";

                    var C17 = j.ToString().PadLeft(2, '0');

                    var _C18 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C18.Add("0"); }
                    var C18 = string.Join("", _C18);

                    var C19 = x.CommissionFrom;

                    var C19A = x.CommissionTo;

                    var C20 = j.ToString().PadLeft(3, '0');

                    var C21 = "H";

                    var C22 = j.ToString().PadLeft(2, '0');

                    var C23 = "A";

                    var C24 = j.ToString().PadLeft(12, '0');

                    var C25 = "A";

                    var _C26 = new List<int>();
                    for (int i = 0; i < 4; i++) { _C26.Add(1 + i); }
                    var C26 = string.Join("", _C26);

                    var _C27 = new List<int>();
                    for (int i = 0; i < 4; i++) { _C27.Add(1 + i); }
                    var C27 = string.Join("", _C27);

                    var C28 = j.ToString().PadLeft(5, '0'); ;

                    var _C29 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C29.Add("0"); }
                    var C29 = string.Join("", _C29);

                    var _C30 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C30.Add("0"); }
                    var C30 = string.Join("", _C30);

                    var C31 = "+";

                    var C32 = "000000,000";

                    var C33 = "01";

                    var _C34 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C34.Add("0"); }
                    var C34 = string.Join("", _C34);

                    var _C35 = new List<string>();
                    for (int i = 0; i < 6; i++) { _C35.Add("0"); }
                    var C35 = string.Join("", _C35);

                    var C36 = j.ToString().PadLeft(6, '0');

                    var C37 = "0000,000"; //304

                    var C38 = "000,00"; //312

                    var C39 = "M";          

                    var C40 = "+";

                    var C41 = "000000,000"; //320

                    var _C42 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C42.Add("0"); }
                    var C42 = string.Join("", _C42);

                    var _C43 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C43.Add("0"); }
                    var C43 = string.Join("", _C43);

                    var _C44 = new List<string>();
                    for (int i = 0; i < 30; i++) { _C44.Add("0"); }
                    var C44 = string.Join("", _C44);

                    var _C45 = new List<string>();
                    for (int i = 0; i < 30; i++) { _C45.Add("0"); }
                    var C45 = string.Join("", _C45);

                    var _C46 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C46.Add("0"); }
                    var C46 = string.Join("", _C46);

                    var _C47 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C47.Add("0"); }
                    var C47 = string.Join("", _C47);

                    var _C48 = new List<int>();
                    for (int i = 0; i < 7; i++) { _C48.Add(1 + i); }
                    var C48 = string.Join("", _C48);

                    var C49 = j.ToString().PadLeft(5, '0');

                    var C50 = j.ToString().PadLeft(6, '0');

                    var C51 = j.ToString().PadLeft(4, '0');

                    var C52 = j.ToString().PadLeft(4, '0');

                    var _C53 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C53.Add("0"); }
                    var C53 = string.Join("", _C53);

                    var _C54 = new List<string>();
                    for (int i = 0; i < 50; i++) { _C54.Add("0"); }
                    var C54 = string.Join("", _C54);

                    var _C55 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C55.Add("0"); }
                    var C55 = string.Join("", _C55);

                    var _C56 = new List<string>();
                    for (int i = 0; i < 150; i++) { _C56.Add("0"); }
                    var C56 = string.Join("", _C56);

                    var _C57 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C57.Add("0"); }
                    var C57 = string.Join("", _C57);

                    var _C58 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C58.Add("0"); }
                    var C58 = string.Join("", _C58);

                    var _C59 = new List<string>();
                    for (int i = 0; i < 4; i++) { _C59.Add("0"); }
                    var C59 = string.Join("", _C59);

                    var C60 = "+";

                    var C61 = "00000,00";

                    var C62 = "+";

                    var C63 = "000000,000";

                    var C64 = x.SetNo;

                    var C65 = "+"; //783

                    var C66 = "000,000"; //784

                    var C66A = "+"; //784

                    var C66B = "000,00000"; //792

                    var _C67 = new List<string>();
                    for (int i = 0; i < 5; i++) { _C67.Add("0"); }
                    var C67 = string.Join("", _C67);

                    var C68 = "00000,00"; //806

                    var C69 = "000000,00"; //814

                    var _C70 = new List<string>();
                    for (int i = 0; i < 10; i++) { _C70.Add("0"); }
                    var C70 = string.Join("", _C70);

                    //var _C71 = new List<string>();
                    //for (int i = 0; i < 16; i++) { _C71.Add("0"); }
                    //var C71 = string.Join("", _C71);
                    var C71 = "A 222 340 47 00 ";

                    //var _C72 = new List<string>();
                    //for (int i = 0; i < 13; i++) { _C72.Add("0"); }
                    //var C72 = string.Join("", _C72);
                    var C72 = "A2223404700  ";

                    var C73 = "00";

                    var _C74 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C74.Add("0"); }
                    var C74 = string.Join("", _C74);

                    var C75 = "J";

                    var _C76 = new List<string>();
                    for (int i = 0; i < 18; i++) { _C76.Add("0"); }
                    var C76 = string.Join("", _C76);

                    var _C77 = new List<string>();
                    for (int i = 0; i < 3; i++) { _C77.Add("0"); }
                    var C77 = string.Join("", _C77);

                    var C78 = "000000,000"; //906

                    var C79 = "000000,00000000"; //916

                    var C79A = "0000";

                    var C80 = "000000,00";

                    var C80A = "0000";

                    var C81 = "00000000,00";

                    var _C82 = new List<string>();
                    for (int i = 0; i < 20; i++) { _C82.Add("0"); }
                    var C82 = string.Join("", _C82);

                    var _C83 = new List<string>();
                    for (int i = 0; i < 122; i++) { _C83.Add("0"); }
                    var C83 = string.Join("", _C83);


                    //var F001 = "010";
                    //var F004 = "001";
                    //var F007 = "0500";
                    //var F011 = "";
                    //var F015 = "";
                    //var F017 = "";
                    //var F021 = "";
                    //var F024 = "";
                    //var F054 = "";
                    //var F062 = "";
                    //var F092 = "";
                    //var F099 = "";
                    //var F106 = "";
                    //var F110 = "";
                    //var F114 = "";
                    //var F117 = "";
                    //var F125 = "";
                    //var F127 = "";
                    //var F130 = "";
                    //var F135 = "";
                    //var F140 = "";
                    //var F143 = "";
                    //var F144 = "";
                    //var F146 = "";
                    //var F147 = "";
                    //var F159 = "";
                    //var F160 = "";
                    //var F164 = "";
                    //var F168 = "";
                    //var F178 = "";
                    //var F223 = "";
                    //var F273 = "";
                    //var F274 = "";
                    //var F284 = "";
                    //var F286 = "";
                    //var F292 = "";
                    //var F298 = "";
                    //var F304 = "";
                    //var F312 = "";
                    //var F318 = "";
                    //var F319 = "";
                    //var F320 = "";
                    //var F330 = "";
                    //var F350 = "";
                    //var F400 = "";
                    //var F430 = "";
                    //var F460 = "";
                    //var F465 = "";
                    //var F469 = "";
                    //var F476 = "";
                    //var F481 = "";
                    //var F487 = "";
                    //var F491 = "";
                    //var F495 = "";
                    //var F545 = "";
                    //var F595 = "";
                    //var F600 = "";
                    //var F750 = "";
                    //var F754 = "";
                    //var F757 = "";
                    //var F761 = "";
                    //var F762 = "";
                    //var F770 = "";
                    //var F771 = "";
                    //var F781 = "";
                    //var F783 = "";
                    //var F784 = "";
                    //var F791 = "";
                    //var F792 = "";
                    //var F801 = "";
                    //var F806 = "";
                    //var F814 = "";
                    //var F823 = "";
                    //var F833 = "";
                    //var F849 = "";
                    //var F862 = "";
                    //var F864 = "";
                    //var F884 = "";
                    //var F885 = "";
                    //var F903 = "";
                    //var F906 = "";
                    //var F916 = "";
                    //var F931 = "";
                    //var F940 = "";
                    //var F951 = "";

                    j++;

                    text.AppendLine($"{C01}{C02}{C03}{C04}{C05}{C06}{C07}" +
                                    $"{C08}{C09}{C10}{C11}{C12}{C13}{C14}" +
                                    $"{C15}{C16}{C17}{C18}{C19}{C19A}{C20}{C21}" +
                                    $"{C22}{C23}{C24}{C25}{C26}{C27}{C28}" +
                                    $"{C29}{C30}{C31}{C32}{C33}{C34}{C35}" + 
                                    $"{C36}{C37}{C38}{C39}{C40}{C41}{C42}" +
                                    $"{C43}{C44}{C45}{C46}{C47}{C48}{C49}" +
                                    $"{C50}{C51}{C52}{C53}{C54}{C55}{C56}" +
                                    $"{C57}{C58}{C59}{C60}{C61}{C62}{C63}" +
                                    $"{C64}{C65}{C66}{C66A}{C66B}{C67}{C68}{C69}{C70}" +
                                    $"{C71}{C72}{C73}{C74}{C75}{C76}{C77}" +
                                    $"{C78}{C79}{C79A}{C80}{C80A}{C81}{C82}{C83}");
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