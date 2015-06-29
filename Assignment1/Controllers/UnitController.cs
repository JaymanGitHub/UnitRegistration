using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Assignment1.Controllers
{
    public class UnitController : Controller
    {

        private GlobalUniDBContext db = new GlobalUniDBContext();

        public ActionResult Index()
        {
            return View(db.Enrolments.ToList());
        }
    }
}
