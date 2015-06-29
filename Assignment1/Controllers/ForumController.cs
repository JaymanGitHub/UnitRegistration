using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    [Authorize(Roles = "Tutor, Lecturer, Student")]
    public class ForumController : Controller
    {
        private GlobalUniDBContext db = new GlobalUniDBContext();

        public ActionResult Index()
        {
            return View(db.Forums.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Forum/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Forum forum)
        {
            if (ModelState.IsValid)
            {
                forum.Name = User.Identity.Name;
                forum.DateCreated = DateTime.Now;
                db.Forums.Add(forum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(forum);
        }


        public ActionResult Delete(int id)
        {
            Forum forum = db.Forums.Find(id);
            db.Forums.Remove(forum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
