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
    public class MasterTypeItemController : Controller
    {
        private readonly db_TaapContext _context;

        public MasterTypeItemController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "master-type-items";
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

        private void GetSelectOptionShop()
        {
            ViewBag.SelectOptionShop = _context.MasterShops
                .Where(p => p.Status.Equals(true))
                .Select(p => new SelectOption
                {
                    Value = p.ShopId.ToString(),
                    Text = p.ShopName
                })
                .Distinct()
                .OrderBy(p => p.Text)
                .ToList();
        }

        // GET: MasterTypeItems
        public async Task<IActionResult> Index()
        {
            ViewBag.MetaForm = GetMetaForm();
            var list = await (from item in _context.MasterTypeItem

                              join type in _context.MasterTypes on item.TypeId equals type.TypeId into a1
                              from mType in a1.DefaultIfEmpty()

                              join shop in _context.MasterShops on item.ShopId equals shop.ShopId into a2
                              from mShop in a2.DefaultIfEmpty()

                              select new ViewMasterTypeItem
                              {
                                  ItemId = item.ItemId,
                                  TypeName = mType.TypeName,
                                  ShopName = mShop.ShopName,
                                  Status = item.Status
                              }).ToListAsync();

            if (list == null)
                return NotFound();

            return View(list);
        }

        // GET: Search
        public async Task<IActionResult> Search(string key)
        {
            ViewBag.MetaForm = GetMetaForm();

            if (key == null)
                return NotFound();

            var list = await (from item in _context.MasterTypeItem

                              join type in _context.MasterTypes on item.TypeId equals type.TypeId into a1
                              from mType in a1.DefaultIfEmpty()

                              join shop in _context.MasterShops on item.ShopId equals shop.ShopId into a2
                              from mShop in a2.DefaultIfEmpty()

                              where (mType.TypeName.Contains(key) || mShop.ShopName.Contains(key))
                              select new ViewMasterTypeItem
                              {
                                  ItemId = item.ItemId,
                                  TypeName = mType.TypeName,
                                  ShopName = mShop.ShopName,
                                  Status = item.Status
                              }).ToListAsync();

            if (list == null)
                return NotFound();

            return View(nameof(Index), list);
        }

        // GET: MasterTypeItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterTypeItem = await (from item in _context.MasterTypeItem

                                        join type in _context.MasterTypes on item.TypeId equals type.TypeId into a1
                                        from mType in a1.DefaultIfEmpty()

                                        join shop in _context.MasterShops on item.ShopId equals shop.ShopId into a2
                                        from mShop in a2.DefaultIfEmpty()

                                        where item.ItemId.Equals(id)
                                        select new MasterTypeItemDetail
                                        {
                                            ItemId = item.ItemId,
                                            TypeName = mType.TypeName,
                                            ShopName = mShop.ShopName,
                                            Status = item.Status,
                                            CreateBy = item.CreateBy,
                                            CreateDate = item.CreateDate,
                                            UpdateBy = item.UpdateBy,
                                            UpdateDate = item.UpdateDate
                                        }).SingleOrDefaultAsync();


            if (masterTypeItem == null)
            {
                return NotFound();
            }

            return View(masterTypeItem);
        }

        // GET: MasterTypeItems/Create
        public IActionResult Create()
        {
            ViewBag.MetaForm = GetMetaForm();

            GetSelectOptionShop();
            GetSelectOptionType();

            return View();
        }

        // POST: MasterTypeItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,TypeId,ShopId,Status,CreateBy,CreateDate")] MasterTypeItem masterTypeItem)
        {

            int userId = int.Parse(masterTypeItem.CreateBy);
            masterTypeItem.CreateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
            masterTypeItem.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(masterTypeItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(masterTypeItem);
        }

        // GET: MasterTypeItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.MetaForm = GetMetaForm();
            if (id == null)
            {
                return NotFound();
            }

            var masterTypeItem = await _context.MasterTypeItem.SingleOrDefaultAsync(m => m.ItemId == id);
            if (masterTypeItem == null)
            {
                return NotFound();
            }

            GetSelectOptionShop();
            GetSelectOptionType();

            return View(masterTypeItem);
        }

        // POST: MasterTypeItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,TypeId,ShopId,Status,CreateBy,CreateDate,UpdateBy,UpdateDate")] MasterTypeItem masterTypeItem)
        {
            if (id != masterTypeItem.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(masterTypeItem.UpdateBy);
                    masterTypeItem.UpdateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                    masterTypeItem.UpdateDate = DateTime.Now;

                    _context.Update(masterTypeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterTypeItemExists(masterTypeItem.ItemId))
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
            return View(masterTypeItem);
        }

        private bool MasterTypeItemExists(int id)
        {
            return _context.MasterTypeItem.Any(e => e.ItemId == id);
        }
    }
}
