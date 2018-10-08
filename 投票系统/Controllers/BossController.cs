using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using 投票系统.Entity;
using 投票系统.Models;
using 投票系统.SYS;

namespace 投票系统.Controllers
{
    public class BossController : Controller
    {
        EF context = new EF();
        // GET: Boss
        public ActionResult zgqr()
        {
           
            if (!string.IsNullOrWhiteSpace(app.username))
            {
                string username = app.username;
                string password = app.password;

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
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                var zg = context.confirm.FirstOrDefault(a => a.state == 0);
                if (zg.source == 1||zg.source==2)
                {
                    return RedirectToRoute(new { Controller = "Boss", Action = "Category1" });
                }

            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public ActionResult Category1()
        {
            if (!string.IsNullOrWhiteSpace(app.username))
            {
                string username = app.username;
                string password = app.password;

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
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public JsonResult tplist1(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "")
        {
            List<product> list = new List<product>();
            list = context.product.Where(a => a.ID > 0).ToList();
            if (!string.IsNullOrWhiteSpace(name))
            {
                list = list.Where(a => a.Name.Contains(name)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                list = list.Where(a => a.Category == category).ToList();
            }
            if (!string.IsNullOrWhiteSpace(groups))
            {
                list = list.Where(a => a.Groups == groups).ToList();
            }
            if (!string.IsNullOrWhiteSpace(fzr))
            {
                list = list.Where(a => a.FZR.Contains(fzr)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                list = list.Where(a => a.state == state).ToList();
            }
            if (context.confirm.FirstOrDefault(a => a.state == 0).source == 1)
            {
                list = list.Where(a => a.TJDJ == "重大课题").ToList();
            }
            else if (context.confirm.FirstOrDefault(a => a.state == 0).source == 2)
            {
                list = list.Where(a => a.TJDJ == "重点课题").ToList();
            }
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<tjtpModel> result = new List<tjtpModel>();
            foreach (var i in list)
            {
                var u = context.users.FirstOrDefault(a => a.UserName == app.username);
                var f = i.tpfs.FirstOrDefault(a => a.MemberId == u.ID);
                tjtpModel p = new tjtpModel();
                p.addDate = i.addDate.ToString();
                p.Category = i.Category;
                p.FZR = i.FZR;
                p.Groups = i.Groups;
                p.Name = i.Name;
                p.Id = i.ID;
                p.BH = i.BH;
                p.tjfl = i.TJDJ;
                p.qrfl = i.QRDJ;
                p.ps = i.tpfs.Where(a => a.lb == 1 && a.fs == 1).Count();
                result.Add(p);
            }
            result = result.OrderByDescending(a => a.ps).ToList();
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult changeTP(int Id, string tj)
        {
            var p = context.product.FirstOrDefault(a => a.ID == Id);
            if (tj == "同意")
            {
                p.QRDJ = p.TJDJ;
            }
            else
            {
                if (p.TJDJ == "重大课题")
                {
                    p.QRDJ = "重点课题";
                }
                else
                {
                    p.QRDJ = "一般课题";
                }
            }
            context.SaveChanges();
            return Json(new { success=true });
        }
        [HttpPost]
        public JsonResult tj(int state)
        {
            if (state == 1)
            {
                var b = context.confirm.FirstOrDefault(a => a.state == 0);
                b.state = 1;
                context.SaveChanges();
                var p = context.product.Where(a => a.TJDJ != a.QRDJ).ToList();
                foreach (var i in p)
                {
                    i.TJDJ = i.QRDJ;
                }
                context.SaveChanges();
            }
            return Json(new { success = true });
        }
    }
}