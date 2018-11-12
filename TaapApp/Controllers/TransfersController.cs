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
    public class TransfersController : Controller
    {
        private readonly db_TaapContext _context;

        public TransfersController(db_TaapContext context)
        {
            _context = context;
        }

        private static string SetPartType(string p) {
            switch(p){
                case "LOC":
                    return p;
                default:
                    return "CKD";
            }
        }

        private string GetStatus(int statusId)
        {
            string _s = null;

            switch (statusId)
            {
                case 0:
                    _s = "Not Update";
                    break;
                case 1:
                    _s = "Is Update";
                    break;
            }

            return _s;
        }

        public string GetMetaForm()
        {
            return "transfer";
        }


        // GET: Transfers
        public IActionResult Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(DateTime dateFrom, DateTime dateTo)
        {

            try{
                var pr = await _context.PartReceive
                                       .Where(x => x.DateToProduction >= dateFrom && x.DateToProduction <= dateTo)
                                       .Select(x => new
                                       {
                                           x.ReceiveNo,
                                           x.DateToProduction,
                                           x.Model,
                                           x.PackingMonth,
                                           x.Consignment,
                                           x.CommissionFrom,
                                           x.CommissionTo,
                                           x.Shop,
                                           PartType = SetPartType(x.PartType)
                                       })
                                       .Distinct()
                                       .ToListAsync();

                var tf = await _context.Transfers
                                       .Where(x => x.DateToProduction >= dateFrom && x.DateToProduction <= dateTo)
                                       .ToListAsync();

                var res = (from _pr in pr
                           join _tf in tf on new { _pr.ReceiveNo, _pr.PartType, _pr.DateToProduction } equals new { _tf.ReceiveNo, _tf.PartType, _tf.DateToProduction } into a1

                           from _a1 in a1.DefaultIfEmpty()

                           select new TransferResponse
                           {
                               TfId = _a1 == null ? 0 : _a1.TfId,
                               ReceiveNo = _pr.ReceiveNo,
                               DateToProduction = _pr.DateToProduction,
                               Model = _pr.Model,
                               PackingMonth = _pr.PackingMonth,
                               Consignment = _pr.Consignment,
                               CommissionTo = _pr.CommissionTo,
                               CommissionFrom = _pr.CommissionFrom,
                               Shop = _pr.Shop,
                               PartType = _pr.PartType,
                               TfNo = _a1 == null ? "" : _a1.TfNo,
                               Status = _a1 == null ? 0 : 1,
                               StatusDesc = _a1 == null ? GetStatus(0) : GetStatus(1)
                           }).ToList()
                             .OrderBy(x => x.TfNo)
                             .ThenBy(x => x.DateToProduction)
                             .ThenBy(x => x.ReceiveNo);

                return Ok(res);

            } catch(Exception ex){
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetTransfer(string receiveNo, DateTime dateToProduction, string partType) {
            try{

                var pr = await _context.PartReceive
                        .Where(x => x.ReceiveNo == receiveNo && x.DateToProduction == dateToProduction && SetPartType(x.PartType) == partType)
                        .Select(x => new
                        {
                            x.ReceiveNo,
                            x.DateToProduction,
                            x.Model,
                            x.PackingMonth,
                            x.Consignment,
                            x.CommissionFrom,
                            x.CommissionTo,
                            x.Shop,
                            PartType = SetPartType(x.PartType)
                        })
                        .Distinct()
                        .ToListAsync();

                var tf = await _context.Transfers
                                       .Where(x => x.ReceiveNo == receiveNo && x.DateToProduction == dateToProduction && x.PartType == partType)
                                       .ToListAsync();

                var res = (from _pr in pr
                           join _tf in tf on new { _pr.ReceiveNo, _pr.PartType, _pr.DateToProduction } equals new { _tf.ReceiveNo, _tf.PartType, _tf.DateToProduction } into a1

                           from _a1 in a1.DefaultIfEmpty()

                           select new TransferResponse
                           {
                               TfId = _a1 == null ? 0 : _a1.TfId,
                               ReceiveNo = _pr.ReceiveNo,
                               DateToProduction = _pr.DateToProduction,
                               Model = _pr.Model,
                               PackingMonth = _pr.PackingMonth,
                               Consignment = _pr.Consignment,
                               CommissionTo = _pr.CommissionTo,
                               CommissionFrom = _pr.CommissionFrom,
                               Shop = _pr.Shop,
                               PartType = _pr.PartType,
                               TfNo = _a1 == null ? "" : _a1.TfNo
                           }).ToList();

                return Ok(res.FirstOrDefault());

            } catch(Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: Transfers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> CreateTransfer(Transfer transfer)
        {

            try
            {
                if (TransferExists(transfer.TfId))
                {
                    _context.Update(transfer);
                    await _context.SaveChangesAsync();
                    return Ok(transfer);
                }
                else
                {
                    _context.Add(transfer);
                    await _context.SaveChangesAsync();
                    return Ok(transfer);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

        }

        private bool TransferExists(int id)
        {
            return _context.Transfers.Any(e => e.TfId == id);
        }
    }
}
