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
    public class MobileScoreCardsController : Controller
    {
        private GolfDB db = new GolfDB();

        // GET: MobileScoreCards
        public ActionResult Index()
        {
            return View();
            //return View(db.MobileScoreCards.ToList());
        }

        // GET: MobileScoreCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileScoreCard mobileScoreCard = db.MobileScoreCards.Find(id);
            if (mobileScoreCard == null)
            {
                return HttpNotFound();
            }
            return View(mobileScoreCard);
        }

        // GET: MobileScoreCards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MobileScoreCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Names,ScoreCardId")] MobileScoreCard mobileScoreCard)
        {
            if (ModelState.IsValid)
            {
                db.MobileScoreCards.Add(mobileScoreCard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mobileScoreCard);
        }

        // GET: MobileScoreCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileScoreCard mobileScoreCard = db.MobileScoreCards.Find(id);
            if (mobileScoreCard == null)
            {
                return HttpNotFound();
            }
            return View(mobileScoreCard);
        }

        // POST: MobileScoreCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Names,ScoreCardId")] MobileScoreCard mobileScoreCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mobileScoreCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mobileScoreCard);
        }

        // GET: MobileScoreCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileScoreCard mobileScoreCard = db.MobileScoreCards.Find(id);
            if (mobileScoreCard == null)
            {
                return HttpNotFound();
            }
            return View(mobileScoreCard);
        }

        // POST: MobileScoreCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MobileScoreCard mobileScoreCard = db.MobileScoreCards.Find(id);
            db.MobileScoreCards.Remove(mobileScoreCard);
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
