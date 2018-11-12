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
    public class MasterTypesController : Controller
    {
        private readonly db_TaapContext _context;

        public MasterTypesController(db_TaapContext context)
        {
            _context = context;
        }
        public string GetMetaForm()
        {
            return "master-types";
        }

        // GET: MasterTypes
        public async Task<IActionResult> Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View(await _context.MasterTypes.ToListAsync());
        }

        // GET: Search
        public async Task<IActionResult> Search(string key)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (key == null)
                return NotFound();

            var masterTypes = await _context.MasterTypes
                .Where(m => m.TypeName.Contains(key))
                .ToListAsync();

            if (masterTypes == null)
                return NotFound();


            return View(nameof(Index), masterTypes);
        }

        // GET: MasterTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterTypes = await _context.MasterTypes
                .SingleOrDefaultAsync(m => m.TypeId == id);
            if (masterTypes == null)
            {
                return NotFound();
            }

            return View(masterTypes);
        }

        // GET: MasterTypes/Create
        public IActionResult Create()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        // POST: MasterTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName,Status,CreateBy,CreateDate")] MasterTypes masterTypes)
        {

            int userId = int.Parse(masterTypes.CreateBy);
            masterTypes.CreateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
            masterTypes.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(masterTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MasterTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterTypes = await _context.MasterTypes.SingleOrDefaultAsync(m => m.TypeId == id);
            if (masterTypes == null)
            {
                return NotFound();
            }
            return View(masterTypes);
        }

        // POST: MasterTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName,Status,CreateBy,CreateDate,UpdateBy,UpdateDate")] MasterTypes masterTypes)
        {
            if (id != masterTypes.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(masterTypes.UpdateBy);
                    masterTypes.UpdateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                    masterTypes.UpdateDate = DateTime.Now;

                    _context.Update(masterTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterTypesExists(masterTypes.TypeId))
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
            return RedirectToAction(nameof(Index));
        }

        private bool MasterTypesExists(int id)
        {
            return _context.MasterTypes.Any(e => e.TypeId == id);
        }
    }
}
