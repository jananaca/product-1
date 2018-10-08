using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Bean;
using WebApplication1.EF;

namespace WebApplication1.Controllers
{
    public class ConsoleController : Controller
    {
        // GET: Console
        /*
         * 
         * 课题上传--新建课题
         * 
         */
        public ActionResult xjkt()
        {
            if (!string.IsNullOrWhiteSpace(Users.username))
            {
                string username = Users.username;
                string password = Users.password;
                EFEntities context = new EF.EFEntities();
                EF.users User = context.users.FirstOrDefault(a => a.UserName == username);
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
                List<EF.category> list = context.category.Where(a=>a.ID>0).ToList();
                List<EF.groups> list1 = context.groups.Where(a => a.privilege1.AddReview == 1).ToList();
                ViewData["category"] = list;
                ViewData["groups"] = list1;
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        /*
         *
         * 课题上传--上传进度
         * 
         */
        // GET: Console/Details/5
        public ActionResult scjd(int page=1)
        {
            if (!string.IsNullOrWhiteSpace(Users.username))
            {
                string username = Users.username;
                string password = Users.password;
                EFEntities context = new EF.EFEntities();
                EF.users User = context.users.FirstOrDefault(a => a.UserName == username);
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
                List<EF.product> list = context.product.Where(a => a.state == "进行中").OrderBy(a => a.addDate).Skip(10 * (page - 1)).Take(10).ToList();
                ViewData["count"] = context.product.Count(a => a.state == "进行中");
                if ((int)ViewData["count"] / 10 * 10 < (int)ViewData["count"])
                {
                    ViewData["count"] = (int)ViewData["count"] / 10 + 1;
                }
                else
                {
                    ViewData["count"] = (int)ViewData["count"] / 10;
                }
                ViewData["data"] = list;
                ViewData["page"] = page;
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
         /*
         *
         * 管理员--进度上传
         * 
         */
        // GET: Console/Create
        public ActionResult jdsc(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Users.username))
            {
                string username = Users.username;
                string password = Users.password;
                EFEntities context = new EF.EFEntities();
                EF.users User = context.users.FirstOrDefault(a => a.UserName == username);
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
                List<EF.product> list = context.product.Where(a => a.state == "进行中" ).OrderBy(a => a.addDate).Skip(10 * (page - 1)).Take(10).ToList();
                ViewData["count"] = context.product.Count(a => a.state == "进行中" );
                if ((int)ViewData["count"] / 10 * 10 < (int)ViewData["count"])
                {
                    ViewData["count"] = (int)ViewData["count"] / 10 + 1;
                }
                else
                {
                    ViewData["count"] = (int)ViewData["count"] / 10;
                }
                ViewData["data"] = list;
                ViewData["page"] = page;
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }

        /*
         *
         * 管理员--用户管理
         * 
         */
        public ActionResult yhgl(FormCollection collection)
        {
            if (!string.IsNullOrWhiteSpace(Users.username))
            {
                string username = Users.username;
                string password = Users.password;
                EFEntities context = new EF.EFEntities();
                EF.users User = context.users.FirstOrDefault(a => a.UserName == username);
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
                ViewData["groups"] = context.groups.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }

        /*
         *
         * 管理员--新建用户
         * 
         */
        public ActionResult xjyh()
        {
            if (!string.IsNullOrWhiteSpace(Users.username))
            {
                string username = Users.username;
                string password = Users.password;
                EFEntities context = new EF.EFEntities();
                EF.users User = context.users.FirstOrDefault(a => a.UserName == username);
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
                ViewData["groups"] = context.groups.Where(a => a.ID > 0).ToList();
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }

        public ActionResult tcdl()
        {
            Users.username = "";
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }

        // POST: Console/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Console/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Console/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
