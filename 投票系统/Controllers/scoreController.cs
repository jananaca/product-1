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
    public class scoreController : Controller
    {
        EF context = new EF();
        // GET: score
        public ActionResult pf()
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
        public JsonResult bmlist(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "", string category1 = "")
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
            if (!string.IsNullOrWhiteSpace(category1))
            {
                list = list.Where(a => a.QRDJ == category1).ToList();
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
            List<dfModel> result = new List<dfModel>();
            foreach (var i in list)
            {
                dfModel d = new dfModel();
                var u = context.users.FirstOrDefault(a => a.UserName == app.username);
                d.BH = i.BH;
                d.name = i.Name;
                var p = i.ktpf.FirstOrDefault(a => a.memberId == u.ID);
                if (p == null)
                {
                    ktpf k = new ktpf();
                    k.dfhj = 0;
                    k.jjxy = 0;
                    k.jsnd = 0;
                    k.jssp = 0;
                    k.memberId = u.ID;
                    k.jjxy = 0;
                    k.jsnd = 0;
                    k.productId = i.ID;
                    k.shxy = 0;
                    k.tgyy = 0;
                    k.zscq = 0;
                    context.ktpf.Add(k);
                    context.SaveChanges();
                    p = i.ktpf.FirstOrDefault(a => a.memberId == u.ID);
                }
                d.dfhj = p.dfhj;
                d.jjxy = p.jjxy;
                d.Id = p.ID;
                d.jsnd = p.jsnd;
                d.jssp = p.jssp;
                d.shxy = p.shxy;
                d.tgyy = p.tgyy;
                d.zscq = p.zscq;
                result.Add(d);
            }
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult changepf(int Id, int state, int value)
        {
            var k = context.ktpf.FirstOrDefault(a => a.ID == Id);
            if (state == 1)
            {
                k.jssp = value;
            }
            else if (state == 2)
            {
                k.jsnd = value;
            }
            else if (state == 3)
            {
                k.zscq = value;
            }
            else if (state == 4)
            {
                k.jjxy = value;
            }
            else if (state == 5)
            {
                k.shxy = value;
            }
            else if (state == 6)
            {
                k.tgyy = value;
            }
            k.dfhj = k.jssp + k.jsnd + k.zscq + k.jjxy + k.shxy + k.tgyy;
            context.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult pfjg()
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
        public JsonResult jglist(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "", string category1 = "")
        {
            List<product> list = new List<product>();
            list = context.product.Where(a => a.ID > 0 ).ToList();
            if (!string.IsNullOrWhiteSpace(name))
            {
                list = list.Where(a => a.Name.Contains(name)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                list = list.Where(a => a.Category == category).ToList();
            }
            if (!string.IsNullOrWhiteSpace(category1))
            {
                list = list.Where(a => a.QRDJ == category1).ToList();
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
            List<dfModel> result = new List<dfModel>();
            foreach (var i in list)
            {
                dfModel d = new dfModel();
                var u = context.users.FirstOrDefault(a => a.UserName == app.username);
                d.BH = i.BH;
                d.name = i.Name;
                
                d.dfhj = i.ktpf.Sum(a=>a.dfhj);
                d.jjxy = i.ktpf.Sum(a => a.jjxy);
                d.Id = i.ID;
                d.jsnd = i.ktpf.Sum(a => a.jsnd);
                d.jssp = i.ktpf.Sum(a => a.jssp);
                d.shxy = i.ktpf.Sum(a => a.shxy);
                d.tgyy = i.ktpf.Sum(a => a.tgyy);
                d.zscq = i.ktpf.Sum(a => a.zscq);
               
                result.Add(d);
            }
            result = result.OrderByDescending(d => d.zscq).ToList();
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult pf1()
        {
            var u = context.users.FirstOrDefault(a => a.UserName == app.username);
            var j = context.productjd.FirstOrDefault(a => a.State != "-1");
            tj t = new Entity.tj();
            t.MemberId = u.ID;
            t.JDId = j.ID;
            t.Date = DateTime.Now;
            t.state = 4;
            context.tj.Add(t);
            context.SaveChanges();
            var l = context.users.Where(a => a.groups.privilege1.Review == 1 && a.State != "-1").ToList();
            var l2 = context.tj.Where(a => a.state == 4 && a.JDId == j.ID).ToList();
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
                j.State = "6";
                j.NextState = "等待全部提交";
                context.SaveChanges();
            }
            return Json(new { success = true });
        }
        public ActionResult hjtj()
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
        public JsonResult tjlist(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "", string category1 = "")
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
            if (!string.IsNullOrWhiteSpace(category1))
            {
                list = list.Where(a => a.QRDJ == category1).ToList();
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
            List<hjtjModel> result = new List<hjtjModel>();
            foreach (var i in list)
            {
                hjtjModel d = new hjtjModel();
                var u = context.users.FirstOrDefault(a => a.UserName == app.username);
                var p = i.pjtj.FirstOrDefault(a=>a.ID>0);
                if (p != null)
                {
                    d.tjfl = p.state;
                }
                else
                {
                    pjtj pj = new pjtj();
                    var jd = context.productjd.FirstOrDefault(a => a.State != "-1");
                    pj.jdId = jd.ID;
                    pj.MemberId = u.ID;
                    pj.productId = i.ID;
                    pj.state = 4;
                    context.pjtj.Add(pj);
                    context.SaveChanges();
                    tp t = new tp();
                    t.state = 0;
                    t.zdstate = 0;
                    t.productId = i.ID;
                    t.MemberId = u.ID;
                    context.tp.Add(t);
                    context.SaveChanges();
                }
                 p = i.pjtj.FirstOrDefault(a => a.ID > 0);
                d.tjfl = p.state;
                p = i.pjtj.FirstOrDefault(a => a.MemberId == u.ID);
                d.BH = i.BH;
                d.name = i.Name;
                d.dfhj = i.ktpf.Sum(a => a.dfhj);
                d.jjxy = i.ktpf.Sum(a => a.jjxy);
                d.tgyy = i.ktpf.Sum(a => a.tgyy);
                d.Id = p.ID;
                d.jsnd = i.ktpf.Sum(a => a.jsnd);
                d.jssp = i.ktpf.Sum(a => a.jssp);
                d.shxy = i.ktpf.Sum(a => a.shxy);
                d.tgyy = i.ktpf.Sum(a => a.tgyy);
                d.zscq = i.ktpf.Sum(a => a.zscq);
                result.Add(d);
            }
            result = result.OrderByDescending(d => d.zscq).ToList();
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult changetj(int Id, int value)
        {
            var p = context.pjtj.FirstOrDefault(a => a.ID == Id);
            p.state = value;
            context.SaveChanges();
            var t = context.tp.FirstOrDefault(a => a.productId == p.productId);
            t.zdstate = value;
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult hj1()
        {
            var u = context.users.FirstOrDefault(a => a.UserName == app.username);
            var j = context.productjd.FirstOrDefault(a => a.State != "-1");
            tj t = new Entity.tj();
            t.MemberId = u.ID;
            t.JDId = j.ID;
            t.Date = DateTime.Now;
            t.state = 5;
            context.tj.Add(t);
            context.SaveChanges();
            var l = context.users.Where(a => a.groups.privilege1.TJ == 1 && a.State == "1").ToList();
            var l2 = context.tj.Where(a => a.state == 5 && a.JDId == j.ID).ToList();
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
                j.State = "7";
                j.NextState = "等待全部提交";
                context.SaveChanges();
            }
            return Json(new { success = true });
        }

    }
}