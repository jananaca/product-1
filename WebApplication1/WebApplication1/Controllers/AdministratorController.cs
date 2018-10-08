using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.EF;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdministratorController : Controller
    {
        EF.EFEntities context = new EFEntities();
        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }
        /*
         * 
         * 课题上传--更新进度--post
         * 
         */
        [HttpPost]
        public string upJD(int Id)
        {
            WebApplication1.EF.product p = context.product.FirstOrDefault(a => a.ID == Id);
            string path = Server.MapPath("~/zl/") + p.Name;
            var file = Request.Files["jd"];
            string date = DateTime.Now.Year + "年" + DateTime.Now.Month + "月";
            file.SaveAs(path + "/" + date + ".doc");
            EF.upjd u = new upjd();
            u.ProductID = Id;
            u.upDate = DateTime.Now.ToString();
            context.upjd.Add(u);
            context.SaveChanges();
            return "success";
        }
        /*
         * 
         * 管理员--进度上传--GET
         * 
         */
        // GET: Administrator/Details/5
        public JsonResult List(int page,string name="",string category="",string groups="",string fzr="",string state="")
        {
            List<EF.product> list = new List<product>();
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
                result.Add(p);
            }
            return Json(new {rows=result, total=total });
        }
        /*
         * 
         * 管理员--新建用户--post
         * 
         */
        [HttpPost]
        // GET: Administrator/Create
        public JsonResult xjyh(string username,string password,string name,string professional,string department,string sex,int group)
        {
            EF.users u = new users();
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
                return Json(new { success = false,msg="用户名已存在" });
            }
        }
        /*
         * 
         * 课题上传--更新进度--post
         * 
         */
        // POST: Administrator/Create
        public JsonResult yhglList(int page, string username="",string department="",string name="",int groups=0,string state="")
        {
            List<EF.users> list = new List<users>();
            list = context.users.Where(a => a.ID > 0).ToList();
            if (!string.IsNullOrWhiteSpace(username))
            {
                list = list.Where(a => a.UserName.Contains(username)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                list = list.Where(a => a.Name.Contains(name)).ToList();
            }
            if (groups!=0&&(!string.IsNullOrWhiteSpace(groups.ToString())))
            {
                list = list.Where(a => a.Group == groups).ToList();
            }
            if (!string.IsNullOrWhiteSpace(state.ToString()))
            {
                list = list.Where(a => a.State==state).ToList();
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
                u.state = i.State=="1"?"正常":"冻结";
                u.sex = i.Sex;
                u.name = i.Name;
                u.group = i.groups.GroupName;
                u.department = i.Department;
                result.Add(u);
            }
            return Json(new { rows = result, total = total });
        }

        // GET: Administrator/Edit/5
        [HttpPost]
        public JsonResult updateyh(int id,int state)
        {
            if (state == 1)
            {
                var i = context.users.FirstOrDefault(a => a.ID == id);
                i.State ="1";
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

        // POST: Administrator/Edit/5
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

        // GET: Administrator/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Administrator/Delete/5
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
