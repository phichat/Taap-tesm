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
    public class MasterModelsController : Controller
    {
        private readonly db_TaapContext _context;

        public MasterModelsController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "master-models";
        }

        // GET: MasterModels
        public async Task<IActionResult> Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            var list = await (from model in _context.MasterModels

                              join type in _context.MasterTypes on model.TypeId equals type.TypeId into a1
                              from mType in a1.DefaultIfEmpty()

                              where mType.Status.Equals(true)
                              select new ViewMasterModels
                              {
                                  ModelId = model.ModelId,
                                  TypeName = mType.TypeName,
                                  ModelName = model.ModelName,
                                  Status = model.Status
                              }).ToListAsync();

            GetSelectOptionType();

            return View(list);
        }

        public async Task<IActionResult> Search(string key)
        {
            ViewBag.MetaForm = GetMetaForm();
            var list = await (from model in _context.MasterModels

                              join type in _context.MasterTypes on model.TypeId equals type.TypeId into a1
                              from mType in a1.DefaultIfEmpty()

                              where mType.Status.Equals(true) && (model.ModelName.Contains(key) || mType.TypeName.Contains(key))
                              select new ViewMasterModels
                              {
                                  ModelId = model.ModelId,
                                  TypeName = mType.TypeName,
                                  ModelName = model.ModelName,
                                  Status = model.Status
                              }).ToListAsync();

            GetSelectOptionType();

            return View(nameof(Index), list);
        }

        private void GetSelectOptionType()
        {
            ViewBag.SelectOptionType = _context.MasterTypes
                .Where(p => p.Status.Equals(true))
                .Select(p => new SelectOption
                {
                    Value = p.TypeId.ToString(),
                    Text = p.TypeName
                })
                .Distinct()
                .OrderBy(p => p.Text)
                .ToList();
        }

        // GET: MasterModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterModels = await _context.MasterModels
                .SingleOrDefaultAsync(m => m.ModelId == id);
            if (masterModels == null)
            {
                return NotFound();
            }

            return View(masterModels);
        }

        // GET: MasterModels/Create
        public IActionResult Create()
        {
            ViewBag.MetaForm = GetMetaForm();
            GetSelectOptionType();
            return View();
        }

        // POST: MasterModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,TypeId,ModelName,Status,CreateBy,CreateDate")] MasterModels masterModels)
        {
            int userId = int.Parse(masterModels.CreateBy);
            masterModels.CreateDate = DateTime.Now;
            masterModels.CreateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;

            if (ModelState.IsValid)
            {  
                _context.Add(masterModels);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(masterModels);
        }

        // GET: MasterModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterModels = await _context.MasterModels.SingleOrDefaultAsync(m => m.ModelId == id);
            if (masterModels == null)
            {
                return NotFound();
            }

            GetSelectOptionType();

            return View(masterModels);
        }

        // POST: MasterModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,TypeId,ModelName,Status,CreateBy,CreateDate,UpdateBy,UpdateDate")] MasterModels masterModels)
        {
            if (id != masterModels.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(masterModels.UpdateBy);
                    
                    masterModels.UpdateDate = DateTime.Now;
                    masterModels.UpdateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;

                    _context.Update(masterModels);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterModelsExists(masterModels.ModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), masterModels);
        }

        private bool MasterModelsExists(int id)
        {
            return _context.MasterModels.Any(e => e.ModelId == id);
        }
    }
}
