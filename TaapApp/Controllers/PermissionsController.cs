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
    public class PermissionsController : Controller
    {
        private readonly db_TaapContext _context;

        public PermissionsController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "permissions";
        }

        // GET: Permissions
        public async Task<IActionResult> Index()
        {
            var list = await (from a in _context.Permissions
                              join b in _context.Roles on a.RoleId equals b.RoleId into b1
                              from f in b1.DefaultIfEmpty()
                              select new PermissionsViewModel
                              {
                                  PermissionId = a.PermissionId,
                                  RoleId = a.RoleId,
                                  RoleName = f.RoleName,
                                  Form = a.Form,
                                  Description = a.Description
                              }).OrderBy(p => p.RoleId).ToListAsync();
            
            GetSelectOptionRoles();
            GetSelectOptionPages();

            ViewBag.MetaForm = GetMetaForm();

            return View(list);
        }

        public async Task<IActionResult> Search(int? role, string form)
        {
            var list = await (from a in _context.Permissions
                              join b in _context.Roles on a.RoleId equals b.RoleId into b1
                              from f in b1.DefaultIfEmpty()
                              select new PermissionsViewModel
                              {
                                  PermissionId = a.PermissionId,
                                  RoleId = a.RoleId,
                                  RoleName = f.RoleName,
                                  Form = a.Form,
                                  Description = a.Description
                              }).OrderBy(p => p.RoleId).ToListAsync();

            GetSelectOptionRoles();
            GetSelectOptionPages();

            if (role != null)
                list = list.Where(p => p.RoleId == role).ToList();

            if (form != null)
                list = list.Where(p => p.Form == form).ToList();

            ViewBag.MetaForm = GetMetaForm();

            return View(nameof(Index), list);
        }

        // GET: Permissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissions = await (from a in _context.Permissions
                                     join b in _context.Roles on a.RoleId equals b.RoleId into b1
                                     from f in b1.DefaultIfEmpty()
                                     where a.PermissionId == id
                                     select new PermissionsViewModel
                                     {
                                         PermissionId = a.PermissionId,
                                         RoleId = a.RoleId,
                                         RoleName = f.RoleName,
                                         Form = a.Form,
                                         Description = a.Description,
                                         Viewer = (a.Viewer == true ? "True" : "False"),
                                         Creater = a.Creater == true ? "True" : "False",
                                         Editer = a.Editer == true ? "True" : "False",
                                         Deleter = a.Deleter == true ? "True" : "False",
                                         Printer = a.Printer == true ? "True" : "False"
                                     })
                                     .Distinct()
                                     .SingleOrDefaultAsync();
            
            if (permissions == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();

            return View(permissions);
        }

        // GET: Permissions/Create
        public IActionResult Create()
        {
            GetSelectOptionRoles();
            GetSelectOptionPages();

            ViewBag.MetaForm = GetMetaForm();

            return View();
        }

        private void GetSelectOptionRoles()
        {
            ViewBag.SelectOptionRoles = _context.Roles
                .Select(p => new SelectOption
                {
                    Value = p.RoleId.ToString(),
                    Text = p.RoleName
                })
                .Distinct()
                .OrderBy(p => p.Value)
                .ToList();
        }

        private void GetSelectOptionPages()
        {
            ViewBag.SelectOptionPages = _context.Permissions
                .Select(p => new SelectOption
                {
                    Value = p.Form,
                    Text = p.Form
                })
                .Distinct()
                .OrderBy(p => p.Value)
                .ToList();
        }


        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermissionId,RoleId,Form,Description,Viewer,Creater,Editer,Deleter,Printer")] Permissions permissions)
        {
            //,Viewer,Creater,Editer,Deleter,Printer
            if (ModelState.IsValid)
            {
                _context.Add(permissions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            GetSelectOptionPages();
            GetSelectOptionRoles();

            ViewBag.MetaForm = GetMetaForm();

            return View(permissions);
        }

        // GET: Permissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetSelectOptionPages();
            GetSelectOptionRoles();

            var permissions = await _context.Permissions.SingleOrDefaultAsync(m => m.PermissionId == id);
            if (permissions == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();

            return View(permissions);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermissionId,RoleId,Form,Description,Viewer,Creater,Editer,Deleter,Printer")] Permissions permissions)
        {
            //,Viewer,Creater,Editer,Deleter,Printer
            if (id != permissions.PermissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permissions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionsExists(permissions.PermissionId))
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

            GetSelectOptionPages();
            GetSelectOptionRoles();

            ViewBag.MetaForm = GetMetaForm();

            return View(permissions);
        }

        // GET: Permissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var permissions = await (from a in _context.Permissions
                                     join b in _context.Roles on a.RoleId equals b.RoleId into b1
                                     from f in b1.DefaultIfEmpty()
                                     where a.PermissionId == id
                                     select new PermissionsViewModel
                                     {
                                         PermissionId = a.PermissionId,
                                         RoleId = a.RoleId,
                                         RoleName = f.RoleName,
                                         Form = a.Form,
                                         Description = a.Description,
                                         Viewer = (a.Viewer == true ? "True" : "False"),
                                         Creater = a.Creater == true ? "True" : "False",
                                         Editer = a.Editer == true ? "True" : "False",
                                         Deleter = a.Deleter == true ? "True" : "False",
                                         Printer = a.Printer == true ? "True" : "False"
                                     })
                                     .Distinct()
                                     .SingleOrDefaultAsync();

            if (permissions == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();

            return View(permissions);
        }

        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permissions = await _context.Permissions.SingleOrDefaultAsync(m => m.PermissionId == id);
            _context.Permissions.Remove(permissions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionsExists(int id)
        {
            return _context.Permissions.Any(e => e.PermissionId == id);
        }

    }
}
