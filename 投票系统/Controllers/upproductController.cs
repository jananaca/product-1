using System;
using System.Collections.Generic;
using System.IO;
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
    public class upproductController : Controller
    {
        EF context = new EF();
        // GET: upproduct
        public ActionResult xjkt()
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
                List<category> list = context.category.Where(a => a.ID > 0).ToList();
                List<groups> list1 = context.groups.Where(a => a.privilege1.AddReview == 1).ToList();
                ViewData["category"] = list;
                ViewData["groups"] = list1;
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        [HttpPost]
        public ActionResult addReview(string category, string name, string group, string fzr)
        {
            string path = Server.MapPath("~/zl/") + name;
            if (Directory.Exists(path))
            {
                return View(0);
            }
            else
            {
                Directory.CreateDirectory(path);
                /*var file = Request.Files["lxsqs"];
                file.SaveAs(path + "/立项申请书.doc");
                file = Request.Files["kfrws"];
                file.SaveAs(path + "/开发任务书.doc");*/
            }
            product p = new product();
            p.Name = name;
            p.userName = app.username;
            p.Category = category;
            p.Groups = group;
            p.FZR = fzr;
            p.filepath = @"/zl/" + name;
            p.addDate = DateTime.Now;
            p.state = "进行中";
            p.BH = "暂无编号";
            p.TJDJ = "暂未推荐";
            p.QRDJ = "暂未确认";
            context.product.Add(p);
            context.SaveChanges();
            return View(1);
        }
        public ActionResult scjd()
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
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public ActionResult ktlb()
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
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public JsonResult List(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "")
        {
            List<product> list = new List<product>();
            list = context.product.Where(a => a.ID > 0&&a.userName==app.username).ToList();
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
        public string upJD(int Id,string lb)
        {
            product p = context.product.FirstOrDefault(a => a.ID == Id);
            string path = Server.MapPath("~/zl/") + p.Name;
            var file = Request.Files["jd"];
            if (lb == "进度")
            {
                string date = DateTime.Now.Year + "年" + DateTime.Now.Month + "月";
                file.SaveAs(path + "/" + date +"."+ file.FileName.Split('.')[file.FileName.Split('.').Count()-1]);
            }
            else
            {
                file.SaveAs(path + "/" + lb + "." + file.FileName.Split('.')[file.FileName.Split('.').Count() - 1]);
            }
            upjd u = new upjd();
            u.ProductID = Id;
            u.upDate = DateTime.Now.ToString();
            context.upjd.Add(u);
            context.SaveChanges();
            return "success";
        }
     
    }
}