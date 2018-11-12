using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaapApp.Models;

namespace TaapApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly db_TaapContext _context;

        public SignInController(db_TaapContext context)
        {
            _context = context;
        }

        // GET: SignIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] ISignIn signIn)
        {
            try
            {
                if (signIn.UserName == null || signIn.Password == null)
                    return NotFound();

                var s = _context.Users.SingleOrDefault(p => p.UserName == signIn.UserName);

                List<UserInfo> userInfo = await (from u in _context.Users
                                                 join r in _context.Roles on u.RoleId equals r.RoleId into r1
                                                 from f in r1.DefaultIfEmpty()
                                                 join p in _context.Permissions on f.RoleId equals p.RoleId into p1
                                                 from g in p1.DefaultIfEmpty()
                                                 where u.UserName == signIn.UserName
                                                 select new UserInfo
                                                 {
                                                     UserId = u.UserId,
                                                     UserName = u.UserName,
                                                     Password = u.Password,
                                                     RoleID = u.RoleId,
                                                     RoleName = f.RoleName,
                                                     PermissionID = g.PermissionId,
                                                     Form = g.Form,
                                                     Viewer = g.Viewer,
                                                     Creater = g.Creater,
                                                     Editer = g.Editer,
                                                     Printer = g.Printer,
                                                     Deleter = g.Deleter
                                                 }).ToListAsync();

                if (userInfo == null)
                    return NotFound();

                string passHash = userInfo[0].Password;

                using (MD5 md5Hash = MD5.Create())
                {
                    if (AuthController.VerifyMd5Hash(md5Hash, signIn.Password, passHash))
                    {
                        var payload = new Dictionary<string, object>();

                        payload.Add("UserId", userInfo[0].UserId);

                        payload.Add("Roles", userInfo.Select(p => new
                        {
                            RoleID = p.RoleID,
                            RoleName = p.RoleName
                        }).Distinct().ToList());

                        payload.Add("Permissions", userInfo.Select(p => new
                        {
                            PermissionId = p.PermissionID,
                            Form = p.Form,
                            Viewer = p.Viewer,
                            Creater = p.Creater,
                            Editer = p.Editer,
                            Printer = p.Printer,
                            Deleter = p.Deleter
                        }).ToList());

                        var token = AuthController.JwtEncoder(payload);
                        var obj = new Dictionary<string, object>
                            {
                                {"access_token", token}
                            };

                        var update = _context.Users.Where(p => p.UserId == userInfo[0].UserId).FirstOrDefault();
                        if (update != null)
                            update.LastLoginDate = DateTime.Now;

                        _context.SaveChanges();

                        Response.Headers.Add("Authorization", token);
                        return Ok(obj);
                    }
                    else
                    {
                        return StatusCode(401);
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

       

        public class ISignIn
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public partial class UserInfo
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int? RoleID { get; set; }
            public string RoleName { get; set; }
            public int? PermissionID { get; set; }
            public string Form { get; set; }
            public bool? Viewer { get; set; }
            public bool? Creater { get; set; }
            public bool? Editer { get; set; }
            public bool? Printer { get; set; }
            public bool? Deleter { get; set; }
        }
    }
}