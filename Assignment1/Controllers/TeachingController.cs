using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    [Authorize(Roles = "Tutor, Lecturer, Student")]
    public class TeachingController : Controller
    {
        private GlobalUniDBContext db = new GlobalUniDBContext();
        // GET: /Teaching/

        public ActionResult Index()
        {
            var items = db.Teachings.ToList();

            //var item = items.Where(k => k.Username == User.Identity.Name);

            return View(items);
        }

        public ActionResult Edit(int id)
        {
            Teaching teaching = db.Teachings.Find(id);
            if (teaching == null)
            {
                return RedirectToAction("Index");
            }
            return View(teaching);
        }

        //
        // POST: /Unit/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Teaching teaching)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teaching).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teaching);
        }


        [HttpPost]
        public ActionResult LecturerUploadFile(HttpPostedFileBase selectedFile, string rename)
        {
            try
            {
                if (selectedFile != null && selectedFile.ContentLength > 0)
                {
                    string guid = Guid.NewGuid().ToString() + Path.GetExtension(selectedFile.FileName);

                    //var fileName = selectedFile.FileName;
                    //var filePath = Path.Combine(Server.MapPath("~/LecturerUploadedFiles/"), fileName);

                    var filePath = Path.Combine(Server.MapPath("~/LecturerUploadedFiles/"), guid);

                    //if (!string.IsNullOrEmpty(rename))
                    //{
                    //    filePath = Path.Combine(Server.MapPath("~/LecturerUploadedFiles/"), rename + Path.GetExtension(selectedFile.FileName));
                    //}

                    selectedFile.SaveAs(filePath);

                    Teaching teaching = new Teaching();
                    teaching.Guid = guid;
                    teaching.Username = User.Identity.Name;

                    if (!string.IsNullOrEmpty(rename))
                    {
                        teaching.RealName = rename + Path.GetExtension(selectedFile.FileName);
                    }
                    else
                    {
                        teaching.RealName = selectedFile.FileName;
                    }

                    teaching.DOC = DateTime.Now;
                    db.Teachings.Add(teaching);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch { ViewBag.Message = "Upload failed"; return RedirectToAction("Index"); }
        }


        public ActionResult DeleteFile(string fileName)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileName))
                {
                    System.IO.File.Delete(Server.MapPath("~/LecturerUploadedFiles/") + fileName);
                }

                return RedirectToAction("Index");
            }
            catch { ViewBag.Message = "Upload failed"; return RedirectToAction("Index"); }
        }

        public ActionResult Delete(int id)
        {
            Teaching teaching = db.Teachings.Find(id);
            DeleteFile(teaching.Guid);
            db.Teachings.Remove(teaching);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
