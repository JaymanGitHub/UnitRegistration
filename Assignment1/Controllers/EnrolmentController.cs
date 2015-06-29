using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment1.Controllers
{
    [Authorize(Roles = "Tutor, Lecturer, Student")]
    public class EnrolmentController : Controller
    {
        //
        // GET: /Enrolment/

        private GlobalUniDBContext db = new GlobalUniDBContext();

        int PreTutorialId = 0;

        public ActionResult Index(string status)
        {
            var items = db.Enrolments.ToList();

            if (!string.IsNullOrEmpty(status))
            {
                ViewBag.Status = status;
            }

            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult View(int tutorialId)
        {
            var items = db.EnrolledStudents.ToList().Where(u => u.TutorialId == tutorialId);
            return View(items);
        }

        //
        // POST: /Unit/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Enrolment enrolment)
        {
            if (ModelState.IsValid)
            {
                enrolment.SrudentCount = 0;
                enrolment.Status = "Available";
                db.Enrolments.Add(enrolment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public ActionResult Edit(int id)
        {
            Enrolment enrolment = db.Enrolments.Find(id);
            if (enrolment == null)
            {
                return RedirectToAction("Index");
            }
            return View(enrolment);
        }

        //
        // POST: /Unit/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Enrolment enrolment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrolment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index");
        }


        public ActionResult Delete(int id)
        {
            Enrolment enrolment = db.Enrolments.Find(id);
            var enrolledStudent = db.EnrolledStudents.Where(m => m.TutorialId == id).ToList();
            foreach (var student in  enrolledStudent)
            {
                DeleteStudent(student.Id);
            }
                db.Enrolments.Remove(enrolment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult DeleteStudent(int id)
        {
            EnrolledStudent enrolledStudent = db.EnrolledStudents.Find(id);
            db.EnrolledStudents.Remove(enrolledStudent);
            db.SaveChanges();

            var enroment = db.Enrolments.Where(u => u.Id == enrolledStudent.TutorialId).FirstOrDefault();
            enroment.SrudentCount = db.EnrolledStudents.Where(u => u.TutorialId == enroment.Id).Count();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Enroll(int tutorialId)
        {
            //var currenttudent = db.EnrolledStudents.Where(u => u.StuName == User.Identity.Name);
            //if(currenttudent.Any())
            //{
            //    var preTutorialId = currenttudent.FirstOrDefault().TutorialId;
            //    var preTutorial = db.Enrolments.Find(preTutorialId);


            //    db.EnrolledStudents.Remove(currenttudent.FirstOrDefault());
            //    db.SaveChanges();
            //    preTutorial.SrudentCount = db.EnrolledStudents.Where(u => u.Id == preTutorialId).Count();
            //    db.SaveChanges();
            //}

            string status = string.Empty;

            EnrolledStudent enrollStudent = new EnrolledStudent();
            var enrolment = db.Enrolments.Find(tutorialId);
            enrollStudent.TutorialId = tutorialId;
            enrollStudent.StuName = User.Identity.Name;
            enrollStudent.DateEnrolled = DateTime.Now;
            //enrolment.SrudentCount = enrolment.SrudentCount + 1;            

            var savedStudent = db.EnrolledStudents.Where(u => u.StuName == User.Identity.Name).FirstOrDefault();
            if (savedStudent != null && savedStudent.Id > 0)
            {
                if (savedStudent.TutorialId == tutorialId)
                {
                    status = "You already enrolled";
                    return RedirectToAction("Index", new { @status = status });
                }

                db.EnrolledStudents.Remove(savedStudent);

                db.SaveChanges();

                //PreEnrolments.SrudentCount = enrolments.
                //var PreEnrolments = db.Enrolments.Find(PreTutorialId);

                //PreEnrolments.SrudentCount = PreEnrolments.SrudentCount - 1;

                var preTutorial = db.Enrolments.Where(e => e.Id == savedStudent.TutorialId).FirstOrDefault();

                var preCount = db.EnrolledStudents.Where(p => p.TutorialId == preTutorial.Id).Count();

                preTutorial.SrudentCount = preCount;

                db.Entry(preTutorial).State = EntityState.Modified;

                db.SaveChanges();
            }

            

            db.EnrolledStudents.Add(enrollStudent);
            db.SaveChanges();

            var count = db.EnrolledStudents.Where(p => p.TutorialId == tutorialId).Count();

            var enrolledStudentsCount = db.EnrolledStudents.Where(p => p.TutorialId == tutorialId).Count();
            enrolment = db.Enrolments.Find(tutorialId);
            enrolment.SrudentCount = count;
            db.SaveChanges();



            PreTutorialId = tutorialId;

            status = "Congratulations! You enrolled successfully!";

            return RedirectToAction("Index", new { @status = status });
        }
    }
}
