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
    public class MasterShopsController : Controller
    {
        private readonly db_TaapContext _context;

        public MasterShopsController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "master-shops";
        }

        // GET: MasterShops
        public async Task<IActionResult> Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View(await _context.MasterShops.ToListAsync());
        }

        // GET: Search
        public async Task<IActionResult> Search(string key)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (key == null)
                return NotFound();

            var masterShops = await _context.MasterShops.Where(p => p.ShopName.Contains(key)).ToListAsync();

            if (masterShops == null)
                return NotFound();

            return View(nameof(Index), masterShops);
        }

        // GET: MasterShops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterShops = await _context.MasterShops
                .SingleOrDefaultAsync(m => m.ShopId == id);
            if (masterShops == null)
            {
                return NotFound();
            }

            return View(masterShops);
        }

        // GET: MasterShops/Create
        public IActionResult Create()
        {
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        // POST: MasterShops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShopId,ShopName,Status,CreateBy,CreateDate")] MasterShops masterShops)
        {

            if (ModelState.IsValid)
            {
                int userId = int.Parse(masterShops.CreateBy);
                masterShops.CreateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                masterShops.CreateDate = DateTime.Now;

                _context.Add(masterShops);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(masterShops);
        }

        // GET: MasterShops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterShops = await _context.MasterShops.SingleOrDefaultAsync(m => m.ShopId == id);
            if (masterShops == null)
            {
                return NotFound();
            }
            return View(masterShops);
        }

        // POST: MasterShops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShopId,ShopName,Status,CreateBy,CreateDate,UpdateBy,UpdateDate")] MasterShops masterShops)
        {
            if (id != masterShops.ShopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(masterShops.UpdateBy);
                    masterShops.UpdateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                    masterShops.UpdateDate = DateTime.Now;

                    _context.Update(masterShops);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterShopsExists(masterShops.ShopId))
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
            return View(masterShops);
        }

        private bool MasterShopsExists(int id)
        {
            return _context.MasterShops.Any(e => e.ShopId == id);
        }
    }
}
