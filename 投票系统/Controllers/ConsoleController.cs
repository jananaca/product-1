using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using 投票系统.Entity;
using 投票系统.SYS;

namespace 投票系统.Controllers
{
    public class ConsoleController : Controller
    {
        // GET: Console
        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(app.username))
            {
                string username = app.username;
                string password = app.password;
                EF context = new EF();
                users User = context.users.FirstOrDefault(a => a.UserName == username);
                byte[] result = Encoding.Default.GetBytes(password);    //tbPass为输入密码的文本框
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string passwordYZ = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
                if (User == null)
                {
                    return RedirectToRoute(new { Controller = "Login", Action = "Index" });
                }
                else if (User.PassWord != passwordYZ.ToLower())
                {
                    return RedirectToRoute(new { Controller = "Login", Action = "Index" });
                }
                List<material> list = context.material.Where(a => a.state == 1).ToList();
                ViewData["Download"] = list;
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
    }
}