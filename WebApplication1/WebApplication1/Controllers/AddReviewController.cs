using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Bean;
using WebApplication1.EF;

namespace WebApplication1.Controllers
{
    public class AddReviewController : Controller
    {
        EF.EFEntities context = new EFEntities();
        // GET: AddReview
        [HttpPost]
        public ActionResult addReview(string category,string name,string group,string fzr)
        {
            string path = Server.MapPath("~/zl/") +  name;
            if (Directory.Exists(path))
            {
                return View(0);
            }
            else
            {
                Directory.CreateDirectory(path);
                var file = Request.Files["lxsqs"];
                file.SaveAs(path + "/立项申请书.doc");
                file= Request.Files["kfrws"];
                file.SaveAs(path + "/开发任务书.doc");
            }
            product p = new product();
            p.Name = name;
            p.userName = Users.username;
            p.Category = category;
            p.Groups = group;
            p.FZR = fzr;
            p.filepath = @"/zl/" + name;
            p.addDate = DateTime.Now;
            p.state = "进行中";
            context.product.Add(p);
            context.SaveChanges();
            return View(1);
        }

        // GET: AddReview/Details/5
        [HttpPost]
        public string upJD(int Id)
        {
            WebApplication1.EF.product p = context.product.FirstOrDefault(a => a.ID == Id);
            string path = Server.MapPath("~/zl/") + p.Name;
            var file = Request.Files["jd"];
            string date = DateTime.Now.Year + "年" + DateTime.Now.Month+"月";
            file.SaveAs(path + "/"+date+".doc");
            EF.upjd u = new upjd();
            u.ProductID = Id;
            u.upDate = DateTime.Now.ToString();
            context.upjd.Add(u);
            context.SaveChanges();
            return "success";
        }

        // GET: AddReview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddReview/Create
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

        // GET: AddReview/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddReview/Edit/5
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

        // GET: AddReview/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddReview/Delete/5
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
