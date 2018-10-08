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
    public class voteController : Controller
    {
        EF context = new EF();
        // GET: vote
        public ActionResult ydjtp()
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
        public JsonResult bmlist(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "", string category1 = "",int jd=0,int lb=1)
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
            if (jd == 1)
            {
                list = list.Where(a => a.tp.FirstOrDefault().zdstate == 1).ToList();
            }
            else if (jd == 2)
            {
                list = list.Where(a => a.tp.FirstOrDefault().zdstate == 2).ToList();
            }
            else if (jd == 3)
            {
                list = list.Where(a => a.tp.FirstOrDefault().zdstate == 3).ToList();
            }
            else if (jd == 4)
            {
                list = list.Where(a => a.tp.FirstOrDefault().zdstate == 0||a.tp.FirstOrDefault().zdstate == 4).ToList();
            }
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<tpModel> result = new List<tpModel>();
            foreach (var i in list)
            {
                tpModel d = new tpModel();
                var u = context.users.FirstOrDefault(a => a.UserName == app.username);
                d.BH = i.BH;
                d.name = i.Name;
                var p = i.ktpf.FirstOrDefault(a => a.memberId == u.ID);
                d.dfhj = p.dfhj;
                d.jjxy = p.jjxy;
                d.Id = i.ID;
                d.jsnd = p.jsnd;
                d.jssp = p.jssp;
                d.shxy = p.shxy;
                d.tgyy = p.tgyy;
                d.zscq = p.zscq;
                var q = i.pjtj.FirstOrDefault(a => a.productId == i.ID);
                if (q.state == 1)
                {
                    d.tjhj = "一等奖";
                }
                if (q.state == 2)
                {
                    d.tjhj = "二等奖";
                }
                if (q.state == 3)
                {
                    d.tjhj = "三等奖";
                }
                var ts = i.tpfs.FirstOrDefault(a => a.lb == lb);
                if (ts == null)
                {
                    tpfs t2 = new tpfs();
                    t2.Date = DateTime.Now;
                    t2.fs = 0;
                    t2.jdId = context.productjd.FirstOrDefault(a => a.State != "-1").ID;
                    t2.lb = lb;
                    t2.MemberId = u.ID;
                    t2.productId = i.ID;
                    context.tpfs.Add(t2);
                    context.SaveChanges();
                    ts = i.tpfs.FirstOrDefault(a => a.lb == lb);
                }
                var t = context.tp.FirstOrDefault(a => a.productId==i.ID);
                if (ts.fs==1)
                {
                    d.state = 1;
                }
                else
                {
                    d.state = 2;
                }
                result.Add(d);
            }
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult change(int Id, int state,int lb)
        {
            var u = context.users.FirstOrDefault(a => a.UserName == app.username);
            var ts = context.product.FirstOrDefault(a => a.ID == Id).tpfs.FirstOrDefault(a => a.lb == lb&&a.MemberId==u.ID);
            ts.fs = state;
            context.SaveChanges();
            return Json(new { success = true });
        }
        public JsonResult ydjtp1(int state)
        {
            var u = context.users.FirstOrDefault(a => a.UserName == app.username);
            var j = context.productjd.FirstOrDefault(a => a.State != "-1");
            tj t = new Entity.tj();
            t.MemberId = u.ID;
            t.JDId = j.ID;
            t.Date = DateTime.Now;
            t.state = state;
            context.tj.Add(t);
            context.SaveChanges();
            var l = context.users.Where(a => a.groups.privilege1.Review == 1 && a.State == "1").ToList();
            var l2 = context.tj.Where(a => a.state == state && a.JDId == j.ID).ToList();
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
        public ActionResult edjtp()
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
        public ActionResult sdjtp()
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

    }
}