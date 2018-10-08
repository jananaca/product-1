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
    public class AdministratorController : Controller
    {
        EF context = new EF();
        // GET: Administrator
        public ActionResult jdsc()
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
        public string upJD(int Id, string lb)
        {
            product p = context.product.FirstOrDefault(a => a.ID == Id);
            string path = Server.MapPath("~/zl/") + p.Name;
            var file = Request.Files["jd"];
            if (lb == "进度")
            {
                string date = DateTime.Now.Year + "年" + DateTime.Now.Month + "月";
                file.SaveAs(path + "/" + date + "." + file.FileName.Split('.')[file.FileName.Split('.').Count() - 1]);
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
        public ActionResult yhgl(FormCollection collection)
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
                ViewData["groups"] = context.groups.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public JsonResult yhglList(int page, string username = "", string department = "", string name = "", int groups = 0, string state = "")
        {
            List<users> list = new List<users>();
            list = context.users.Where(a => a.ID > 0).ToList();
            if (!string.IsNullOrWhiteSpace(username))
            {
                list = list.Where(a => a.UserName.Contains(username)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                list = list.Where(a => a.Name.Contains(name)).ToList();
            }
            if (groups != 0 && (!string.IsNullOrWhiteSpace(groups.ToString())))
            {
                list = list.Where(a => a.Group == groups).ToList();
            }
            if (!string.IsNullOrWhiteSpace(state.ToString()))
            {
                list = list.Where(a => a.State == state).ToList();
            }
            if (!string.IsNullOrWhiteSpace(department))
            {
                list = list.Where(a => a.Department.Contains(department)).ToList();
            }
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<userModel> result = new List<userModel>();
            foreach (var i in list)
            {
                userModel u = new userModel();
                u.Id = i.ID;
                u.username = i.UserName;
                u.state = i.State == "1" ? "正常" : "冻结";
                u.sex = i.Sex;
                u.name = i.Name;
                u.group = i.groups.GroupName;
                u.department = i.Department;
                result.Add(u);
            }
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult updateyh(int id, int state)
        {
            if (state == 1)
            {
                var i = context.users.FirstOrDefault(a => a.ID == id);
                i.State = "1";
                context.SaveChanges();
            }
            if (state == 2)
            {
                var i = context.users.FirstOrDefault(a => a.ID == id);
                i.State = "2";
                context.SaveChanges();
            }
            if (state == 3)
            {
                var i = context.users.FirstOrDefault(a => a.ID == id);
                context.users.Remove(i);
                context.SaveChanges();
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult xjyh_d(string username, string password, string name, string professional, string department, string sex, int group)
        {
            users u = new users();
            if (context.users.FirstOrDefault(a => a.UserName == username) == null)
            {
                u.UserName = username;
                byte[] result = Encoding.Default.GetBytes(password);    //tbPass为输入密码的文本框
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(result);
                string passwordYZ = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
                u.PassWord = passwordYZ.ToLower();
                u.Name = name;
                u.Professional = professional;
                u.Department = department;
                u.Sex = sex;
                u.Group = group;
                u.State = "1";
                u.RegionDate = DateTime.Now;
                context.users.Add(u);
                context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, msg = "用户名已存在" });
            }
        }
        public ActionResult xjyh()
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
                ViewData["groups"] = context.groups.Where(a => a.ID > 0).ToList();
                ViewData["category"] = context.category.Where(a => a.ID > 0).ToList();
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public ActionResult tpjskz()
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
                productjd p = context.productjd.FirstOrDefault(a => a.State != "-1");
                if (p == null)
                {
                    productjd pj = new productjd();
                    pj.YEAR = DateTime.Now.Year.ToString();
                    pj.State = "0";
                    context.productjd.Add(pj);
                    context.SaveChanges();
                    ViewData["JD"] = pj;
                }
                else
                {
                    ViewData["JD"] = p;
                }
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });

        }
        [HttpPost]
        public JsonResult updatejd()
        {
            productjd p = context.productjd.FirstOrDefault(a => a.State != "-1");
            if (p.State == "0")
            {
                p.State = "1";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "1")
            {
                p.State = "2";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "2")
            {
                p.State = "3";
                p.NextState = "等待总工确认";
            }
            else if (p.State == "3")
            {
                p.State = "4";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "4")
            {
                p.State = "5";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "5")
            {
                p.State = "6";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "6")
            {
                p.State = "7";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "7")
            {
                p.State = "8";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "8")
            {
                p.State = "9";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "9")
            {
                p.State = "10";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "10")
            {
                p.State = "11";
                p.NextState = "等待全部提交";
            }
            else if (p.State == "11")
            {
                p.State = "12";
                p.NextState = "等待总工确认";
            }
            else if (p.State == "12")
            {
                p.State = "13";
                p.NextState = "开始下一轮";
            }
            else if (p.State == "13")
            {
                p.State = "-1";
                productjd pj = new productjd();
                pj.YEAR = DateTime.Now.Year.ToString();
                pj.State = "0";
                pj.NextState = "结束征集，开始推荐";
                context.productjd.Add(pj);
            }
            context.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult qx()
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
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public JsonResult qxList(int page)
        {
            List<privilege> list = new List<privilege>();
            list = context.privilege.Where(a => a.ID > 0).ToList();
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<qxModel> result = new List<qxModel>();
            foreach (var i in list)
            {
                qxModel q = new qxModel();
                q.Id = i.ID;
                q.name = i.Name;
                q.addReview = (int)i.AddReview;
                q.Administrator = (int)i.Administrator;
                q.Review = (int)i.Review;
                q.Technical = (int)i.Technical;
                q.TJ = (int)i.TJ;
                q.ZG = (int)i.ZG;
                result.Add(q);
            }
            return Json(new { rows = result, total = total });
        }
        public JsonResult qxChange(int Id, int index)
        {
            if (index == 1)
            {
                privilege p = context.privilege.FirstOrDefault(a => a.ID == Id);
                p.AddReview = (p.AddReview + 1) % 2;
                context.SaveChanges();
            }
            else if (index == 2)
            {
                privilege p = context.privilege.FirstOrDefault(a => a.ID == Id);
                p.Review = (p.Review + 1) % 2;
                context.SaveChanges();
            }
            else if (index == 3)
            {
                privilege p = context.privilege.FirstOrDefault(a => a.ID == Id);
                p.Administrator = (p.Administrator + 1) % 2;
                context.SaveChanges();
            }
            else if (index == 4)
            {
                privilege p = context.privilege.FirstOrDefault(a => a.ID == Id);
                p.Technical = (p.Technical + 1) % 2;
                context.SaveChanges();
            }
            else if (index == 5)
            {
                privilege p = context.privilege.FirstOrDefault(a => a.ID == Id);
                p.TJ = (p.TJ + 1) % 2;
                context.SaveChanges();
            }
            else if (index == 6)
            {
                privilege p = context.privilege.FirstOrDefault(a => a.ID == Id);
                p.ZG = (p.ZG + 1) % 2;
                context.SaveChanges();
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult addqx(string name)
        {
            var q = context.privilege.FirstOrDefault(a => a.Name == name);
            if (q != null)
            {
                return Json(new { success = false, msg = "权限名存在" });
            }
            else
            {
                privilege p = new privilege();
                p.Name = name;
                p.Review = 0;
                p.AddReview = 0;
                p.Administrator = 0;
                p.Technical = 0;
                p.TJ = 0;
                p.ZG = 0;
                context.privilege.Add(p);
                context.SaveChanges();
                return Json(new { success = true });
            }
        }
        public JsonResult delqx(int Id)
        {
            var q = context.privilege.FirstOrDefault(a => a.ID==Id);

            context.privilege.Remove(q);
            context.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult yhz()
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
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        public JsonResult yhzList(int page)
        {
            List<groups> list = new List<groups>();
            list = context.groups.Where(a => a.ID > 0).ToList();
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<yhzModel> result = new List<yhzModel>();
            foreach (var i in list)
            {
                yhzModel y = new yhzModel();
                y.name = i.GroupName;
                y.Id = i.ID;
                y.pri = i.privilege1.Name;
                var apl = context.privilege.Where(a => a.ID > 0).ToList();
                List<string> sl = new List<string>();
                foreach (var j in apl)
                {
                    sl.Add(j.Name);
                }
                y.ap = string.Join(",", sl.ToArray());
                result.Add(y);
            }
            return Json(new { rows = result, total = total });
        }
        [HttpPost]
        public JsonResult yhzChange(int id, string vl)
        {
            var p = context.privilege.FirstOrDefault(a => a.Name == vl);
            var g = context.groups.FirstOrDefault(a => a.ID == id);
            g.Privilege = p.ID;
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult delyhz(int id)
        {
            var g = context.groups.FirstOrDefault(a => a.ID == id);
            context.groups.Remove(g);
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult addyhz(string name)
        {
            groups g = new groups();
            g.GroupName = name;
            g.Privilege = context.privilege.FirstOrDefault(a => a.ID > 0).ID;
            context.groups.Add(g);
            context.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult flgl()
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
                return View(User);
            }
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
        [HttpPost]
        public JsonResult delfl(int id)
        {
            var g = context.category.FirstOrDefault(a => a.ID == id);
            context.category.Remove(g);
            context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult addfl(string name)
        {
            category g = new category();
            g.CategoryName = name;
            context.category.Add(g);
            context.SaveChanges();
            return Json(new { success = true });
        }
        public JsonResult flList(int page)
        {
            List<category> list = new List<category>();
            list = context.category.Where(a => a.ID > 0).ToList();
            int total = list.Count;
            list = list.Skip((page - 1) * 10).Take(10).ToList();
            List<flModel> result = new List<flModel>();
            foreach (var i in list)
            {
                flModel y = new flModel();
                y.name = i.CategoryName;
                y.Id = i.ID;
                result.Add(y);
            }
            return Json(new { rows = result, total = total });
        }
        public ActionResult ktbm()
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
        public JsonResult bmlist(int page, string name = "", string category = "", string groups = "", string fzr = "", string state = "",string category1="")
        {
            List<product> list = new List<product>();
            list = context.product.Where(a => a.ID > 0 && a.userName == app.username).ToList();
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
        public JsonResult changebm(int Id, string tp)
        {
            var p = context.product.FirstOrDefault(a => a.ID == Id);
            p.BH = tp;
            context.SaveChanges();
            return Json(new { success = true });
        }
    }
}