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
    public class MainHomeController : Controller
    {
        // GET: Index
        public ActionResult Index()
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
                List<EF.material> list = context.material.Where(a => a.state == 1).ToList();
                ViewData["Download"] = list;
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }

        // GET: Index/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Index/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Index/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Index/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Index/Edit/5
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

        // GET: Index/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Index/Delete/5
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
