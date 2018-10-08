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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(Response.Cookies["username"].Value))
            {
                string username = Response.Cookies["username"].Value;
                string password = Response.Cookies["password"].Value;
                EF context = new EF();
                users User = context.users.FirstOrDefault(a => a.UserName == username);
                byte[] result = Encoding.Default.GetBytes(password);    //tbPass为输入密码的文本框
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string passwordYZ = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
                if (User == null)
                {
                    return View();
                }
                else if (User.PassWord != passwordYZ.ToLower())
                {
                    return View();
                }
                app.username = username;
                app.password = password;
                return RedirectToRoute(new { Controller = "MainHome", Action = "Index" });

            }
            return View();

        }

        // GET: Login/Details/5
        [HttpPost]
        public ActionResult loginreview(string username, string password)
        {
            EF context = new EF();
            users User = context.users.FirstOrDefault(a => a.UserName == username && a.State == "1");
            byte[] result = Encoding.Default.GetBytes(password);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string passwordYZ = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
            if (User == null)
            {
                return RedirectToRoute(new { Controller = "Login", Action = "UserNameErr" });
            }
            else if (User.PassWord != passwordYZ.ToLower())
            {
                return RedirectToRoute(new { Controller = "Login", Action = "PassWordErr" });
            }
            Response.Cookies["username"].Value = User.UserName;
            Response.Cookies["username"].Expires = DateTime.Now.AddDays(15);
            Response.Cookies["password"].Value = password;
            Response.Cookies["password"].Expires = DateTime.Now.AddDays(15);
            app.username = username;
            app.password = password;

            return RedirectToRoute(new { Controller = "Console", Action = "Index" });
        }
        string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 

                pwd = pwd + s[i].ToString("X");

            }
            return pwd;
        }
        // GET: Login/Create
        public ActionResult UserNameErr()
        {
            return View();
        }
        public ActionResult PassWordErr()
        {
            return View();
        }
    }
}