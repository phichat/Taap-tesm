using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TaapApp.Models;

namespace TaapApp.Controllers
{

    public class ReceiveReferencesController : Controller
    {
        private readonly db_TaapContext _context;
        private IHostingEnvironment _hostingEnv;

        public ReceiveReferencesController(db_TaapContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnv = hostingEnvironment;
        }

        public string GetMetaForm()
        {
            return "buy-off";
        }

        private string GetStatus(int statusId)
        {
            string _s = null;

            switch (statusId)
            {
                case 0:
                    _s = "In Process";
                    break;
                case 1:
                    _s = "Is Update";
                    break;
                case 2:
                    _s = "Not Update";
                    break;
            }

            return _s;
        }

        // GET: ReceiveReferences
        public IActionResult Index()
        {
            ViewBag.SelectOptionReceiveNo = (from p in _context.ReceiveReference
                                             orderby p.ReceiveNo descending
                                             select new SelectOptionReceiveNo
                                             {
                                                 Value = p.ReceiveNo,
                                                 Text = p.ReceiveNo
                                             }).Distinct().ToList();
            ViewBag.ReceiveReference = new List<BuyOffViewModel>();

            ViewBag.MetaForm = GetMetaForm();

            return View();
        }

        // GET: ReceiveReferences/ReceiveReference
        public IActionResult ReceiveReference()
        {
            ViewBag.SelectOptionReceiveNo = (from p in _context.ReceiveReference
                                             select new SelectOptionReceiveNo
                                             {
                                                 Value = p.ReceiveNo,
                                                 Text = p.ReceiveNo
                                             }).Distinct().ToList();
            ViewBag.ReceiveReference = new List<BuyOffViewModel>();
            return View();
        }

        // GET: ReceiveReferences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _ref = await _context.ReceiveReference.SingleOrDefaultAsync(m => m.Vdoid == id);
            
            if (_ref == null)
            {
                return NotFound();
            }


            ViewBag.MetaForm = GetMetaForm();
            return View(_ref);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string receiveNo, string commissionNo, string vdoNo)
        {
            try
            {

                if (receiveNo == null && commissionNo == null && vdoNo == null)
                    return NotFound();

                List<ReceiveReferenceListView> ds = new List<ReceiveReferenceListView>();

                ds = await (from a in _context.ReceiveReference
                            orderby a.SetNo, a.CommissionNo
                            select new ReceiveReferenceListView
                            {
                                Status = (int)a.Status,
                                StatusDesc = GetStatus((int)a.Status),
                                Vdoid = a.Vdoid,
                                SetNo = a.SetNo,
                                ReceiveNo = a.ReceiveNo,
                                CommissionNo = a.CommissionNo,
                                TransferNoCkd = a.TransferNoCkd,
                                TransferNoLoc = a.TransferNoLoc,
                                Vdono = a.Vdono,
                                Date = a.Date,
                                DateToProduction = null
                            }).ToListAsync();

                if (receiveNo != null)
                    ds = ds.Where(p => p.ReceiveNo.Equals(receiveNo)).ToList();

                if (commissionNo != null)
                {
                    var partRec = (from part in _context.PartReceive
                                   where (
                                    int.Parse(commissionNo) >= int.Parse(part.CommissionFrom) &&
                                    int.Parse(commissionNo) <= int.Parse(part.CommissionTo)
                                   )
                                   select new
                                   {
                                       BuyOffRecNo = part.BuyOffRecNo,
                                       commissionForm = part.CommissionFrom,
                                       commissionTo = part.CommissionTo
                                   }).FirstOrDefault();

                    ds = (from com in ds
                          where int.Parse(com.CommissionNo) >= int.Parse(partRec.commissionForm) &&
                                int.Parse(com.CommissionNo) <= int.Parse(partRec.commissionTo)
                          select com
                    ).ToList();
                }

                if (vdoNo != null)
                    ds = ds.Where(p => p.Vdono.Equals(vdoNo)).ToList();

                return Json(ds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool CheckingShop(string commission, string model)
        {
            var partReceives = _context.PartReceive
                .FromSql($"select distinct Model, Shop from dbo.PartReceive where {commission} between CommissionFrom and CommissionTo")
                .Where(item => item.Model == model)
                .Select(item => new
                {
                    model = item.Model,
                    shop = item.Shop
                });

            // var model = partReceives.FirstOrDefault().model;

            var items = (from _model in _context.MasterModels
                         join item in _context.MasterTypeItem on _model.TypeId equals item.TypeId into a1
                         from mitem in a1.DefaultIfEmpty()

                         where _model.ModelName == model
                         select new
                         {
                             modelId = _model.ModelId,
                             typeId = mitem.TypeId,
                             shopId = mitem.ShopId
                         }).Distinct().ToList();

            return items.Count() == partReceives.Count() ? true : false;
        }

        private ReceiveReferenceListView SetItemWithStatus(int statusId, string commissionNo)
        {
            ReceiveReferenceListView r = new ReceiveReferenceListView();
            r = (from a in _context.ReceiveReference
                 where a.CommissionNo == commissionNo
                 select new ReceiveReferenceListView
                 {
                     Status = statusId,
                     StatusDesc = GetStatus(statusId),
                     Vdoid = a.Vdoid,
                     SetNo = a.SetNo,
                     ReceiveNo = a.ReceiveNo,
                     CommissionNo = a.CommissionNo,
                     TransferNoCkd = a.TransferNoCkd,
                     TransferNoLoc = a.TransferNoLoc,
                     Vdono = a.Vdono,
                     Date = a.Date
                 }).FirstOrDefault();

            return r;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveBuyOffFile()
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    IFormFile file = Request.Form.Files[0];

                    if (file.Length <= 0)
                    {
                        return NotFound();
                    }
                    string sWebRootFolder = _hostingEnv.WebRootPath;
                    string sFileName = @"uploads\receiveBuyOff.xlsx";
                    FileInfo fileInfo = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                    using (FileStream fs = System.IO.File.Create(fileInfo.ToString()))
                    {
                        await file.CopyToAsync(fs);

                        ExcelPackage package = new ExcelPackage(fs);
                        StringBuilder sb = new StringBuilder();
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                        int rowCount = worksheet.Dimension.Rows;
                        bool bHeaderRow = true;

                        List<ReceiveReferenceListView> refListView = new List<ReceiveReferenceListView>();
                        int inProcess = 0;
                        int totalCount = 0;
                        int notMatch = 0;
                        int isMatch = 0;

                        for (int row = 1; row <= rowCount; row++)
                        {
                            if (bHeaderRow == true)
                            {
                                bHeaderRow = false;
                                continue;
                            }

                            totalCount++;
                            var model = worksheet.Cells[row, 2].Text.Trim().ToString();
                            var commissionno = worksheet.Cells[row, 5].Text.Trim().ToString();
                            commissionno = commissionno.Replace(" ", string.Empty);

                            // 1. ตรวจสอบว่า Model นี้อยู่ในประเภทรถอะไร
                            // 2. ตรวจสอบว่า ประเภทรถ นี้ต้องผ่านกี่ Shop
                            // 3. เปรียบเทียบว่า ชิ้นส่วนที่อัพโหลดเข้าไปครบทุก Shop ตามเงื่อนไขหรือยัง 
                            if (!CheckingShop(commissionno, model))
                            {
                                // In process
                                refListView.Add(SetItemWithStatus(0, commissionno));
                                inProcess++;
                                continue;
                            }
                            // ค้นหาข้อมูล รายการที่รอ buyoff ด้วยเลข commission เพื่อนำไป update ในลำดับต่อไป
                            var receiveReference = _context.ReceiveReference.SingleOrDefault(p => p.CommissionNo == commissionno);

                            // if receiveReference is not all update
                            if (receiveReference == null)
                            {
                                refListView.Add(SetItemWithStatus(2, commissionno));
                                notMatch++;
                                continue;
                            }

                            // UPDATE ReceiveReference
                            // worksheet.Cells[row, 2] = Model
                            receiveReference.ProductionNo = worksheet.Cells[row, 3].Text.Trim().ToString();
                            receiveReference.PaintTrim = worksheet.Cells[row, 4].Text.Trim().ToString();
                            // worksheet.Cells[row, 5] = CommissionNo
                            receiveReference.ChassisNo = worksheet.Cells[row, 6].Text.Trim().ToString();
                            receiveReference.EngineNo = worksheet.Cells[row, 7].Text.Trim().ToString();
                            receiveReference.Vdono = worksheet.Cells[row, 8].Text.Trim().ToString();
                            // dateTime dd/mm/yyyy => format sql server
                            receiveReference.Date = DateTime.Parse(worksheet.Cells[row, 9].Text.ToString());
                            receiveReference.Status = 1;

                            await _context.SaveChangesAsync();

                            // Is update
                            isMatch++;
                            refListView.Add(SetItemWithStatus(1, commissionno));

                            // Find PartReceive with commissionNo for Update PartReceive
                            var partReceive = await _context.PartReceive
                                .Where(item =>
                                   int.Parse(commissionno) >= int.Parse(item.CommissionFrom) &&
                                   int.Parse(commissionno) <= int.Parse(item.CommissionTo)
                                )
                                .Select(item => new PartReceive
                                {
                                    ReceiveId = item.ReceiveId,
                                    Amount = item.Amount,
                                    CommissionFrom = item.CommissionFrom,
                                    CommissionTo = item.CommissionTo,
                                    Qty = item.Qty
                                })
                                .ToListAsync();


                            // UPDATE PartReceive Amount;
                            if (partReceive.Count() > 0)
                            {
                                var commandText = string.Empty;
                                foreach (var item in partReceive)
                                {
                                    if (item.Amount == 0)
                                        continue;
                                    // ดึงเอา จำนวนรายการ Commission No ออกมา 
                                    // เช่น 0787927411 - 0787927416 + ตัวมันเอง = 6 รายการ (Commission Item)
                                    // 1. (Qty / Commission Item) = qtyByItem (Qty ต่อรายการ Commission)
                                    // 2. (จำนวนคงเหลือ - qtyByItem) = จำนวนคงเหลือ
                                    int comFrom = int.Parse(item.CommissionFrom);
                                    int comTo = int.Parse(item.CommissionTo);
                                    int comItem = 1 + (comTo - comFrom);
                                    int qtyByItem = item.Qty / comItem;

                                    commandText += $" UPDATE dbo.PartReceive SET Amount={item.Amount - qtyByItem}";
                                    commandText += $" WHERE ReceiveId={item.ReceiveId}";
                                }
                                if (commandText != "")
                                    await _context.Database.ExecuteSqlCommandAsync(commandText);
                            }
                        }

                        trans.Commit();
                        return Json(refListView);

                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return StatusCode(500, ex.Message);
                }
            }
        }


        // GET: ReceiveReferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _ref = await _context.ReceiveReference.SingleOrDefaultAsync(m => m.Vdoid == id);

            ViewBag.InProcess = CheckingShop(_ref.CommissionNo, _ref.ModelType);
            ViewBag.MetaForm = GetMetaForm();
            return View(_ref);
        }

        // POST: ReceiveReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Vdoid,Date,Vdono,ReferenceNo,ModelType,LotNo,ProductionNo,PaintTrim,CommissionNo,ReceiveNo,ChassisNo,EngineNo,Remark,ComItem,Status,SetNo, TransferNoLoc,TransferNoCkd")] ReceiveReference receiveReference)
        {
            if (id != receiveReference.Vdoid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (receiveReference.Status == 0)
                    {
                        // Find PartReceive with ReceiveNo for Update PartReceive
                        var update = _context.PartReceive
                            .Where(p => p.BuyOffRecNo == receiveReference.ReceiveNo &&
                               (int.Parse(receiveReference.CommissionNo) >= int.Parse(p.CommissionFrom) &&
                                   int.Parse(receiveReference.CommissionNo) <= int.Parse(p.CommissionTo))
                            ).ToList();

                        // UPDATE PartReceive Amount;
                        if (update.Count() > 0)
                        {
                            foreach (var item in update)
                            {
                                if (item.Amount == 0)
                                    continue;
                                // ดึงเอา จำนวนรายการ Commission No ออกมา 
                                // เช่น 0787927411 - 0787927416 = 6 รายการ (Commission Item)
                                // 1. (Qty / Commission Item) = qtyByItem (Qty ต่อรายการ Commission)
                                // 2. (จำนวนคงเหลือ - qtyByItem) = จำนวนคงเหลือ
                                int qtyByItem = item.Qty / (int)(receiveReference.ComItem);
                                item.Amount = item.Amount - qtyByItem;
                            }

                            receiveReference.Status = 1;
                        }
                    }

                    _context.Update(receiveReference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiveReferenceExists(receiveReference.Vdoid))
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
            ViewBag.MetaForm = GetMetaForm();
            return View(receiveReference);
        }

        private bool ReceiveReferenceExists(int id)
        {
            return _context.ReceiveReference.Any(e => e.Vdoid == id);
        }

    }
}