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
    public class ScoreCardsController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: ScoreCards
        public ActionResult Index()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View(db.ScoreCards.ToList());
        }

        // GET: ScoreCards/Details/5
        public ActionResult Details(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreCard scoreCard = db.ScoreCards.Find(id);
            if (scoreCard == null)
            {
                return HttpNotFound();
            }
            return View(scoreCard);
        }

        // GET: ScoreCards/Create
        public ActionResult Create()
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            return View();
        }

        // POST: ScoreCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HoleList,StartingHole,Team,Division,TeamName")] ScoreCard scoreCard)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.ScoreCards.Add(scoreCard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoreCard);
        }

        // GET: ScoreCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreCard scoreCard = db.ScoreCards.Find(id);
            if (scoreCard == null)
            {
                return HttpNotFound();
            }
            return View(scoreCard);
        }

        // POST: ScoreCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HoleList,StartingHole,Team,Division,TeamName")] ScoreCard scoreCard)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (ModelState.IsValid)
            {
                db.Entry(scoreCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoreCard);
        }

        // GET: ScoreCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(User.IsInRole("CourseAdmin") || User.IsInRole("Admin")))
                return RedirectToAction("../Account/Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreCard scoreCard = db.ScoreCards.Find(id);
            if (scoreCard == null)
            {
                return HttpNotFound();
            }
            return View(scoreCard);
        }

        // POST: ScoreCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoreCard scoreCard = db.ScoreCards.Find(id);
            db.ScoreCards.Remove(scoreCard);
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
