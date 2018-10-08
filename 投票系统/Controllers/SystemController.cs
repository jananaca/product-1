using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 投票系统.SYS;

namespace 投票系统.Controllers
{
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult tcdl()
        {
            app.username = "";
            return RedirectToRoute(new { Controller = "Login", Action = "Index" });
        }
    }
}