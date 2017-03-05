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
    public class CourseRatingsController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: CourseRatings
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View(db.CourseRatings.ToList());
        }

        // GET: CourseRatings/Details/5
        public ActionResult Details(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.CourseRating courseRating = db.CourseRatings.Find(id);
            if (courseRating == null)
            {
                return HttpNotFound();
            }

            return View(courseRating);
        }

        // GET: CourseRatings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId,TeeName,Course_Rating,SlopeRating18,Front9,Back9,BogeyRating,Gender,HandicapByHole,teeBoxMenuColorsId,HoleListId")] GolfDB2.Models.CourseRating courseRating)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.CourseRatings.Add(courseRating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseRating);
        }

        // GET: CourseRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.CourseRating courseRating = db.CourseRatings.Find(id);
            if (courseRating == null)
            {
                return HttpNotFound();
            }
            return View(courseRating);
        }

        // POST: CourseRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,TeeName,Course_Rating,SlopeRating18,Front9,Back9,BogeyRating,Gender,HandicapByHole,teeBoxMenuColorsId,HoleListId")] CourseRating courseRating)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.Entry(courseRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseRating);
        }

        // GET: CourseRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GolfDB2.Models.CourseRating courseRating = db.CourseRatings.Find(id);
            if (courseRating == null)
            {
                return HttpNotFound();
            }
            return View(courseRating);
        }

        // POST: CourseRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GolfDB2.Models.CourseRating courseRating = db.CourseRatings.Find(id);
            db.CourseRatings.Remove(courseRating);
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
