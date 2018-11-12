using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaapApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace TaapApp.Controllers
{
    public class PartReceivesController : Controller
    {
        private readonly db_TaapContext _context;
        private IHostingEnvironment _hostingEnv;

        public PartReceivesController(db_TaapContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnv = hostingEnvironment;
        }

        public string GetMetaForm()
        {
            return "parts-receive";
        }

        //GET: PartReceives
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
        public async Task<IActionResult> Search(string receiveNo, DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                List<PartReceive> partReceive = new List<PartReceive>();
                partReceive = await _context.PartReceive
                    .Where(p => p.ReceiveNo.Equals(receiveNo)).ToListAsync();

                if (dateFrom != null && dateTo != null)
                    partReceive = partReceive
                        .Where(p => p.DateToProduction >= dateFrom && p.DateToProduction <= dateTo).ToList();

                return Json(partReceive);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: PartReceives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partReceive = await _context.PartReceive
                .SingleOrDefaultAsync(m => m.ReceiveId == id);
            if (partReceive == null)
            {
                return NotFound();
            }
            ViewBag.MetaForm = GetMetaForm();

            return View(partReceive);
        }

        // GET: PartReceives/Create
        public IActionResult Create()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }


        // POST: PartReceives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiveId,ReceiveNo,PackingMonth,Model,Consignment,CommissionFrom,CommissionTo,Shop,CustomEntryNo,InvoiceNo,DateToProduction,PartNo,PartDescription,Qty,Amount,Qpv,Um,PartType")] PartReceive partReceive)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partReceive);
                await _context.SaveChangesAsync();
                var id = partReceive.ReceiveId;
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(partReceive);
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveFile()
        {
            try
            {
                IFormFile files = Request.Form.Files[0];

                if (files.Length <= 0)
                {
                    return NotFound();
                }

                string sWebRootFolder = _hostingEnv.WebRootPath;
                string sFileName = @"uploads\PartsReceive.xlsx";
                FileInfo fileInfo = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                using (FileStream fs = new FileStream(fileInfo.ToString(), FileMode.Create))
                {
                    files.CopyTo(fs);
                    using (ExcelPackage package = new ExcelPackage(fs))
                    {
                        StringBuilder sb = new StringBuilder();
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                        int rowCount = worksheet.Dimension.Rows;
                        bool bHeaderRow = true;

                        var partReceive = new List<PartReceive>();

                        for (int row = 1; row <= rowCount; row++)
                        {
                            if (bHeaderRow == true)
                            {
                                bHeaderRow = false;
                                continue;
                            }

                            if (worksheet.Cells[row, 1].Value == null)
                                continue;

                            var packingMonth = worksheet.Cells[row, 1].Text.ToString();
                            var yyyy = packingMonth.Substring(0, 4);
                            var mm = packingMonth.Substring(4, 2);
                            var model = worksheet.Cells[row, 2].Text.ToString();
                            var shop = worksheet.Cells[row, 6].Text.ToString().Substring(0, 1);
                            var Consignment = worksheet.Cells[row, 3].Text.ToString();
                            var Qty = int.Parse(worksheet.Cells[row, 12].Text.ToString());

                            var recNo = mm + yyyy + model + shop + Consignment;
                            var buyOffRecNo = packingMonth + model;
                            partReceive.Add(new PartReceive
                            {
                                ReceiveNo = recNo,
                                BuyOffRecNo = buyOffRecNo,
                                PackingMonth = packingMonth,
                                Model = worksheet.Cells[row, 2].Text.Trim().ToString(),
                                Consignment = worksheet.Cells[row, 3].Text.Trim().ToString(),
                                CommissionFrom = worksheet.Cells[row, 4].Text.Trim().ToString(),
                                CommissionTo = worksheet.Cells[row, 5].Text.Trim().ToString(),
                                Shop = worksheet.Cells[row, 6].Text.Trim().ToString(),
                                CustomEntryNo = worksheet.Cells[row, 7].Text.Trim().ToString(),
                                InvoiceNo = worksheet.Cells[row, 8].Text.Trim().ToString(),
                                DateToProduction = DateTime.Parse(worksheet.Cells[row, 9].Text.ToString() , new CultureInfo("en")),
                                PartNo = worksheet.Cells[row, 10].Text.Trim().ToString(),
                                PartDescription = worksheet.Cells[row, 11].Text.Trim().ToString(),
                                Qty = Qty,
                                Amount = Qty,
                                Qpv = 0,
                                Um = worksheet.Cells[row, 13].Text.Trim().ToString(),
                                PartType = worksheet.Cells[row, 14].Text.Trim().ToString()
                            });

                        }

                        // Find commissionFrom and commissionTo for Add
                        var a = partReceive.FirstOrDefault();
                        int comFrom = int.Parse(a.CommissionFrom);
                        int comTo = int.Parse(a.CommissionTo);
                        int comItem = 1 + (comTo - comFrom);

                        // Find PartReceive and ReceiveReference for Remove
                        var listPartReceive = _context.PartReceive
                            .Where(m => m.ReceiveNo == a.ReceiveNo).ToList();
                        var receiveReferences = (from db in _context.ReceiveReference
                                                 where db.ReceiveNo == a.BuyOffRecNo &&
                                                 (int.Parse(db.CommissionNo) >= comFrom && int.Parse(db.CommissionNo) <= comTo)
                                                 select db).ToList();

                        // Remove
                        if (listPartReceive != null)
                            _context.PartReceive.RemoveRange(listPartReceive);

                        if (receiveReferences != null)
                            _context.ReceiveReference.RemoveRange(receiveReferences);
                        // End Remove

                        // จะมีกรณีที่ partNo เดียวกัน แต่ DateToProduction คนละวัน 
                        // จำเป็นต้องทำการ SUM Qty และเอา DateToProduction วันล่าสุด
                        partReceive = (from rec in partReceive
                                       group rec by new
                                       {
                                           rec.ReceiveNo,
                                           rec.BuyOffRecNo,
                                           rec.PackingMonth,
                                           rec.Model,
                                           rec.Consignment,
                                           rec.CommissionFrom,
                                           rec.CommissionTo,
                                           rec.Shop,
                                           rec.CustomEntryNo,
                                           rec.InvoiceNo,
                                           rec.PartNo,
                                           rec.PartDescription,
                                           rec.Qpv,
                                           rec.Um,
                                           rec.PartType
                                       } into g
                                       select new PartReceive
                                       {
                                           ReceiveNo = g.Key.ReceiveNo,
                                           BuyOffRecNo = g.Key.BuyOffRecNo,
                                           PackingMonth = g.Key.PackingMonth,
                                           Model = g.Key.Model,
                                           Consignment = g.Key.Consignment,
                                           CommissionFrom = g.Key.CommissionFrom,
                                           CommissionTo = g.Key.CommissionTo,
                                           Shop = g.Key.Shop,
                                           CustomEntryNo = g.Key.CustomEntryNo,
                                           InvoiceNo = g.Key.InvoiceNo,
                                           DateToProduction = g.Max(x => x.DateToProduction),
                                           PartNo = g.Key.PartNo,
                                           PartDescription = g.Key.PartDescription,
                                           Qty = g.Sum(x => x.Qty),
                                           Amount = g.Sum(x => x.Amount),
                                           Qpv = (g.Sum(x => x.Qty) / comItem),
                                           Um = g.Key.Um,
                                           PartType = g.Key.PartType
                                       }).ToList();


                        var listReceiveReferences = new List<ReceiveReference>();
                        for (int i = comFrom; i <= comTo; i++)
                        {
                            listReceiveReferences.Add(new ReceiveReference
                            {
                                CommissionNo = i.ToString("0000000000"),
                                ModelType = a.Model,
                                SetNo = a.Consignment,
                                ReceiveNo = a.BuyOffRecNo,
                                Status = 0,
                                ComItem = comItem
                            });
                        }

                        // Add
                        _context.PartReceive.AddRange(partReceive);
                        // Add
                        _context.ReceiveReference.AddRange(listReceiveReferences);

                        // Save
                        await _context.SaveChangesAsync();

                        return Json(partReceive);
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        // GET: PartReceives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partReceive = await _context.PartReceive.SingleOrDefaultAsync(m => m.ReceiveId == id);
            if (partReceive == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();
            return View(partReceive);
        }

        // POST: PartReceives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReceiveId,ReceiveNo,PackingMonth,Model,Consignment,CommissionFrom,CommissionTo,Shop,CustomEntryNo,InvoiceNo,DateToProduction,PartNo,PartDescription,Qty,Amount,Qpv,Um,PartType")] PartReceive partReceive)
        {
            if (id != partReceive.ReceiveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partReceive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartReceiveExists(partReceive.ReceiveId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(partReceive);
        }

        // GET: PartReceives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partReceive = await _context.PartReceive.SingleOrDefaultAsync(m => m.ReceiveId == id);
            if (partReceive == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();
            return View(partReceive);
        }

        // POST: PartReceives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partReceive = await _context.PartReceive.SingleOrDefaultAsync(m => m.ReceiveId == id);
            var itemPartReceive = _context.PartReceive.Where(o => o.ReceiveNo == partReceive.ReceiveNo).Count();

            if (itemPartReceive == 1)
            {
                var receiveRef = _context.ReceiveReference
                    .Where(o => o.ReceiveNo == partReceive.BuyOffRecNo &&
                    (int.Parse(o.CommissionNo) >= int.Parse(partReceive.CommissionFrom) &&
                    int.Parse(o.CommissionNo) <= int.Parse(partReceive.CommissionTo))
                    ).ToList();

                _context.ReceiveReference.RemoveRange(receiveRef);
            }

            _context.PartReceive.Remove(partReceive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteByRec")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedByRec(string ReceiveNo)
        {
            var partReceive = await _context.PartReceive.Where(m => m.ReceiveNo == ReceiveNo).ToListAsync();
            var partRec = partReceive
                .Select(p => new
                {
                    BuyOffRecNo = p.BuyOffRecNo,
                    CommissionFrom = p.CommissionFrom,
                    CommissionTo = p.CommissionTo
                }).FirstOrDefault();

            var receiveRef = await _context.ReceiveReference
                .Where(o => o.ReceiveNo == partRec.BuyOffRecNo &&
                    (int.Parse(o.CommissionNo) >= int.Parse(partRec.CommissionFrom) &&
                    int.Parse(o.CommissionNo) <= int.Parse(partRec.CommissionTo))
                    ).ToListAsync();

            _context.PartReceive.RemoveRange(partReceive);
            _context.ReceiveReference.RemoveRange(receiveRef);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartReceiveExists(int id)
        {
            return _context.PartReceive.Any(e => e.ReceiveId == id);
        }
    }
}
