using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaapApp.Models;
using System.Text;
using System.Security.Cryptography;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace TaapApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly db_TaapContext _context;
        const string secret = "Taap@2018";

        public UsersController(db_TaapContext context)
        {
            
            _context = context;
        }

        public string GetMetaForm()
        {
            return "users";
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await (from a in _context.Users
                               join b in _context.Roles on a.RoleId equals b.RoleId into b2
                               from f in b2.DefaultIfEmpty()
                               select new UsersViewModel
                               {
                                   UserId = a.UserId,
                                   UserName = a.UserName,
                                   Password = a.Password,
                                   FirstName = a.FirstName,
                                   LastName = a.LastName,
                                   Email = a.Email,
                                   CreateBy = a.CreateBy,
                                   CreateDate = a.CreateDate,
                                   UpdateBy = a.UpdateBy,
                                   UpdateDate = a.UpdateDate,
                                   LastLoginDate = a.LastLoginDate,
                                   RoleId = a.RoleId,
                                   RoleName = f.RoleName == null ? "" : f.RoleName,
                               }).ToListAsync();
            ViewBag.MetaForm = GetMetaForm();
            return View(users);
        }

        public async Task<IActionResult> Search(string key)
        {
            var users = await (from a in _context.Users
                               join b in _context.Roles on a.RoleId equals b.RoleId into b2
                               from f in b2.DefaultIfEmpty()
                               where a.UserName.Contains(key) || a.FirstName.Contains(key) ||
                               a.LastName.Contains(key) || f.RoleName.Contains(key)
                               select new UsersViewModel
                               {
                                   UserId = a.UserId,
                                   UserName = a.UserName,
                                   Password = a.Password,
                                   FirstName = a.FirstName,
                                   LastName = a.LastName,
                                   Email = a.Email,
                                   CreateBy = a.CreateBy,
                                   CreateDate = a.CreateDate,
                                   UpdateBy = a.UpdateBy,
                                   UpdateDate = a.UpdateDate,
                                   LastLoginDate = a.LastLoginDate,
                                   RoleId = f.RoleId,
                                   RoleName = f.RoleName,
                               }).ToListAsync();

            ViewBag.MetaForm = GetMetaForm();

            return View(nameof(Index), users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await GetUsersViewModel(id);

            if (users == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();
            return View(users);
        }


        public async Task<IActionResult> ProfileDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await GetUsersViewModel(id);

            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            GetSelectOptionRoles();
            ViewBag.MetaForm = GetMetaForm();
            return View();
        }

        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await GetUsersViewModel(id);

            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,FirstName,LastName,Email,CreateBy,CreateDate,RoleId")] Users users)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(users.CreateBy);
                MD5 md5Hash = MD5.Create();

                users.CreateBy = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                users.CreateDate = DateTime.Now;
                users.Password = AuthController.GetMd5Hash(md5Hash, users.Password);

                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = users.UserId });
            }

            GetSelectOptionRoles();
            ViewBag.MetaForm = GetMetaForm();
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }
            GetSelectOptionRoles();
            ViewBag.MetaForm = GetMetaForm();
            return View(users);
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

        private async Task<UsersViewModel> GetUsersViewModel(int? id)
        {         
            try
            {
                var users = await (from a in _context.Users
                                   join b in _context.Roles on a.RoleId equals b.RoleId into b2
                                   from f in b2.DefaultIfEmpty()
                                   select new UsersViewModel
                                   {
                                       UserId = a.UserId,
                                       UserName = a.UserName,
                                       Password = a.Password,
                                       FirstName = a.FirstName,
                                       LastName = a.LastName,
                                       Email = a.Email,
                                       CreateBy = a.CreateBy,
                                       CreateDate = a.CreateDate,
                                       UpdateBy = a.UpdateBy,
                                       UpdateDate = a.UpdateDate,
                                       LastLoginDate = a.LastLoginDate,
                                       RoleId = a.RoleId,
                                       RoleName = f.RoleName == null ? "-" : f.RoleName,
                                   }).SingleOrDefaultAsync( p => p.UserId == id);

                return users;
            } catch(Exception)
            {
                return null;
            }
            
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,Password,FirstName,LastName,Email,CreateBy,CreateDate,UpdateBy,UpdateDate,LastLoginDate,RoleId")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(users.UpdateBy);
                    var userUpdate = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                    var oUsers = _context.Users.SingleOrDefault(p => p.UserId == id);
                    MD5 md5Hash = MD5.Create();

                    oUsers.UserName = users.UserName;
                    oUsers.Password = (oUsers.Password == users.Password)
                        ? oUsers.Password
                        : AuthController.GetMd5Hash(md5Hash, users.Password);
                    oUsers.FirstName = users.FirstName;
                    oUsers.LastName = users.LastName;
                    oUsers.Email = users.Email;
                    oUsers.UpdateBy = userUpdate;
                    oUsers.UpdateDate = DateTime.Now;
                    oUsers.RoleId = users.RoleId;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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


            GetSelectOptionRoles();
            ViewBag.MetaForm = GetMetaForm();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int id, [Bind("UserId,UserName,Password,FirstName,LastName,Email,UpdateBy,UpdateDate")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = int.Parse(users.UpdateBy);
                    var userUpdate = _context.Users.SingleOrDefault(p => p.UserId == userId).UserName;
                    var oUsers = _context.Users.SingleOrDefault(p => p.UserId == id);
                    MD5 md5Hash = MD5.Create();

                    oUsers.UserName = users.UserName;
                    oUsers.Password = (oUsers.Password == users.Password)
                        ? oUsers.Password
                        : AuthController.GetMd5Hash(md5Hash, users.Password);
                    oUsers.FirstName = users.FirstName;
                    oUsers.LastName = users.LastName;
                    oUsers.Email = users.Email;
                    oUsers.UpdateBy = userUpdate;
                    oUsers.UpdateDate = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProfileDetails), new { id = id });
            }
            
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await GetUsersViewModel(id);

            if (users == null)
            {
                return NotFound();
            }

            ViewBag.MetaForm = GetMetaForm();
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        [HttpPost]
        public IActionResult SignIn([FromBody] SignIn signin)
        {
            if (signin == null)
            {
                return NotFound();
            }

            //List<CustomerInfo> customer;
            //var customer = await _context.Users.SingleOrDefaultAsync(p => p.Password);

            return View();
        }

        private static string JwtEncoder(Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private static bool JwtDecoder(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                return true;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }
            return true;
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public interface SignIn
    {
        string UserName { get; set; }
        string Password { get; set; }
    }

    public interface CustomerInfo
    {
        int UserId { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        //public string MacAddress { get; set; }
    }
}
