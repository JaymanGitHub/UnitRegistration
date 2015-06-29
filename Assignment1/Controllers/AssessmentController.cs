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
    public class AssessmentController : Controller
    {
        private GlobalUniDBContext db = new GlobalUniDBContext();

        public ActionResult Index()
        {
            

            if (User.IsInRole("lecturer") || User.IsInRole("tutor"))
            {
                var allItem = db.Assessments.ToList();
                return View(allItem);
            }
            else {
                var item = db.Assessments.ToList().Where(a => a.StuName == User.Identity.Name);
                return View(item);
            }

            
        }

        public ActionResult Grade(int id = 0)
        {
            Assessment assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }
            return View(assessment);
        }

        //
        // POST: /Unit/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grade(Assessment assessment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assessment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assessment);
        }

        public ActionResult DeleteFile(string fileName)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileName))
                {
                    System.IO.File.Delete(Server.MapPath("~/StudentAssessmentFiles/") + fileName);
                }

                return RedirectToAction("Index");
            }
            catch { ViewBag.Message = "Upload failed"; return RedirectToAction("Index"); }
        }

        public ActionResult Delete(int id)
        {
            Assessment assessment = db.Assessments.Find(id);
            DeleteFile(assessment.Guid);
            db.Assessments.Remove(assessment);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult StudentAssessmentFile(HttpPostedFileBase selectedFile, string rename)
        {
            try
            {
                if (selectedFile != null && selectedFile.ContentLength > 0)
                {
                    string guid = Guid.NewGuid().ToString() + Path.GetExtension(selectedFile.FileName);

                    //var fileName = selectedFile.FileName;
                    //var filePath = Path.Combine(Server.MapPath("~/LecturerUploadedFiles/"), fileName);

                    var filePath = Path.Combine(Server.MapPath("~/StudentAssessmentFiles/"), guid);

                    //if (!string.IsNullOrEmpty(rename))
                    //{
                    //    filePath = Path.Combine(Server.MapPath("~/LecturerUploadedFiles/"), rename + Path.GetExtension(selectedFile.FileName));
                    //}


                    selectedFile.SaveAs(filePath);

                    Assessment assessment = new Assessment();
                    assessment.Guid = guid;
                    assessment.StuName = User.Identity.Name;
                    if (!string.IsNullOrEmpty(rename))
                    { assessment.FileName = rename + Path.GetExtension(selectedFile.FileName); }
                    else
                    { assessment.FileName = selectedFile.FileName; }
                    assessment.DateUpload = DateTime.Now;
                    assessment.Grade = "uploaded for Grading";
                    db.Assessments.Add(assessment);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch { ViewBag.Message = "Upload failed"; return RedirectToAction("Index"); }
        }

    }
}
