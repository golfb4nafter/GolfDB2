using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GolfDB2.Models;

namespace GolfDB2.Controllers
{
    public class CourseDatasController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: CourseDatas
        public ActionResult Index()
        {
            return View(db.CourseDatas.ToList());
        }

        // GET: CourseDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseData courseData = db.CourseDatas.Find(id);
            if (courseData == null)
            {
                return HttpNotFound();
            }
            return View(courseData);
        }

        // GET: CourseDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseName,Address1,Address2,City,State,PostalCode,Email,Phone,Url,GoogleMapUrl,NumberOfHoles,NumberOfNines")] CourseData courseData)
        {
            if (ModelState.IsValid)
            {
                db.CourseDatas.Add(courseData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseData);
        }

        // GET: CourseDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseData courseData = db.CourseDatas.Find(id);
            if (courseData == null)
            {
                return HttpNotFound();
            }
            return View(courseData);
        }

        // POST: CourseDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseName,Address1,Address2,City,State,PostalCode,Email,Phone,Url,GoogleMapUrl,NumberOfHoles,NumberOfNines")] CourseData courseData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseData);
        }

        // GET: CourseDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseData courseData = db.CourseDatas.Find(id);
            if (courseData == null)
            {
                return HttpNotFound();
            }
            return View(courseData);
        }

        // POST: CourseDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseData courseData = db.CourseDatas.Find(id);
            db.CourseDatas.Remove(courseData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
