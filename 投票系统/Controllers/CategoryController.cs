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
    public class CategoryController : Controller
    {
        EF context = new EF();
        // GET: Category
        public ActionResult fltj()
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
        public JsonResult List(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "")
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
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<ProductModel> result = new List<ProductModel>();
            foreach (var i in list)
            {
                ProductModel p = new ProductModel();
                p.addDate = i.addDate.ToString();
                p.Category = i.Category;
                p.FZR = i.FZR;
                p.Groups = i.Groups;
                p.Name = i.Name;
                p.Id = i.ID;
                p.BH = i.BH;
                p.tjfl = i.TJDJ;
                p.qrfl = i.QRDJ;
                result.Add(p);
            }
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult changeTJ(int Id,string tj)
        {
            var p = context.product.FirstOrDefault(a => a.ID == Id);
            p.TJDJ = tj;
            p.QRDJ = tj;
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult tj()
        {
            var u = context.users.FirstOrDefault(a => a.UserName == app.username);
            var j = context.productjd.FirstOrDefault(a => a.State == "1");
            tj t = new Entity.tj();
            t.MemberId = u.ID;
            t.JDId = j.ID;
            t.Date = DateTime.Now;
            t.state = 1;
            context.tj.Add(t);
            context.SaveChanges();
            var l = context.users.Where(a => a.groups.privilege1.TJ == 1&&a.State=="1").ToList();
            var l2 = context.tj.Where(a => a.state == 1 && a.JDId == j.ID).ToList();
            bool flag = false;
            foreach (var i in l)
            {
                bool f2 = false;
                foreach (var k in l2)
                {
                    if (k.MemberId == i.ID)
                    {
                        f2 =true;
                        break;
                    }
                }
                if (f2 == false)
                {
                    flag = true;
                }
            }
            if (flag == false)
            {
                j = context.productjd.FirstOrDefault(a => a.State != "-1");
                j.State = "2";
                j.NextState = "等待全部提交";
                context.SaveChanges();
            }
            return Json(new { success = true });

        }
        public ActionResult tjtp()
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
        public JsonResult tpList(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "")
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
            if (context.productjd.FirstOrDefault(a => a.State != "-1").State == "2")
            {
               list= list.Where(a => a.TJDJ == "重大课题").ToList();
            }
            else
            {
               list= list.Where(a => a.TJDJ == "重点课题").ToList();
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
                
                if (f == null)
                {
                    p.fs = 3;
                }
                else
                {
                    p.fs = f.fs;
                }
                if (p.fs == 1)
                {
                    p.fsname = "同意";
                }
                else if (p.fs == 2)
                {
                    p.fsname = "反对";
                }
                else
                {
                    p.fsname = "弃权";
                }
                result.Add(p);
            }
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult changeTP(int Id, int tp)
        {
            var u= context.users.FirstOrDefault(a => a.UserName == app.username);
            var f = context.tpfs.FirstOrDefault(a => a.MemberId == u.ID && a.productId == Id);
            if (f == null)
            {
                tpfs t = new tpfs();
                var j = context.productjd.FirstOrDefault(a => a.State != "-1");
                t.Date = DateTime.Now;
                t.fs = tp;
                t.jdId = j.ID;
                t.lb = 1;
                t.MemberId = u.ID;
                t.productId = Id;
                context.tpfs.Add(t);
                context.SaveChanges();
            }
            else
            {
                f.fs = tp;
                context.SaveChanges();
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult tp1()
        {
            var u = context.users.FirstOrDefault(a => a.UserName == app.username);
            var j = context.productjd.FirstOrDefault(a => a.State !="-1");
            tj t = new Entity.tj();
            t.MemberId = u.ID;
            t.JDId = j.ID;
            t.Date = DateTime.Now;
            if (j.State == "2")
            {
                t.state = 2;
            }
            else
            {
                t.state = 3;
            }
            context.tj.Add(t);
            context.SaveChanges();
            var l = context.users.Where(a => a.groups.privilege1.Review == 1 && a.State == "1").ToList();
            List<tj> l2 = new List<Entity.tj>();
            if (j.State == "2")
            {
                l2 = context.tj.Where(a => a.state == 2 && a.JDId == j.ID).ToList();
            }
            else
            {
                l2 = context.tj.Where(a => a.state == 3 && a.JDId == j.ID).ToList();
            }
            bool flag = false;
            foreach (var i in l)
            {
                bool f2 = false;
                foreach (var k in l2)
                {
                    if (k.MemberId == i.ID)
                    {
                        f2 = true;
                        break;
                    }
                }
                if (f2 == false)
                {
                    flag = true;
                }
            }
            if (flag == false)
            {
                j = context.productjd.FirstOrDefault(a => a.State != "-1");
                if (j.State == "2")
                {
                    j.State = "2.5";
                    j.NextState = "等待全部提交";
                }
                else
                {
                    j.State = "3";
                    j.NextState = "等待总工确认";
                }
               
                context.SaveChanges();
                confirm c = new confirm();
                c.addDate = DateTime.Now;
                c.jdId = j.ID;
                if (j.State == "2.5")
                {
                    c.source = 1;
                }
                else
                {
                    c.source = 2;
                }
                c.state = 0;
                context.confirm.Add(c);
                context.SaveChanges();
            }
            return Json(new { success = true });

        }
        public ActionResult tpjg()
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
            return Json(new { rows = result, total = total });
        }
    }
}