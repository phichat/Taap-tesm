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
    public class RolesController : Controller
    {
        private readonly db_TaapContext _context;

        public RolesController(db_TaapContext context)
        {
            _context = context;
        }

        public string GetMetaForm()
        {
            return "roles";
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            GetSelectOptionRoles();
            ViewBag.MetaForm = GetMetaForm();
            return View(await _context.Roles.OrderBy(p => p.RoleId).ToListAsync());
        }

        public async Task<IActionResult> Search(int? role)
        {
            GetSelectOptionRoles();
            var list = await _context.Roles
                .OrderBy(p => p.RoleId)
                .ToListAsync();

            if (role != null)
                list = list.Where(p => p.RoleId == role).ToList();

            ViewBag.MetaForm = GetMetaForm();

            return View(nameof(Index), list);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles
                .SingleOrDefaultAsync(m => m.RoleId == id);
            if (roles == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();
            return View(roles);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
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

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,CreateBy,CreateDate")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(roles.CreateBy);
                roles.CreateDate = DateTime.Now;
                roles.CreateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;

                _context.Add(roles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MetaForm = GetMetaForm();
            return View(roles);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles.SingleOrDefaultAsync(m => m.RoleId == id);
            if (roles == null)
            {
                return NotFound();
            }
            ViewBag.MetaForm = GetMetaForm();
            return View(roles);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,UpdateBy,UpdateDate")] Roles roles)
        {
            if (id != roles.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(roles.UpdateBy);
                    string roleName = roles.RoleName;

                    roles = _context.Roles.SingleOrDefault(p => p.RoleId == id);
                    roles.RoleName = roleName;
                    roles.UpdateDate = DateTime.Now;
                    roles.UpdateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolesExists(roles.RoleId))
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
            ViewBag.MetaForm = GetMetaForm();
            return View(roles);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles
                .SingleOrDefaultAsync(m => m.RoleId == id);
            if (roles == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();
            return View(roles);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roles = await _context.Roles.SingleOrDefaultAsync(m => m.RoleId == id);
            _context.Roles.Remove(roles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolesExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
