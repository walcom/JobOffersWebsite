using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobOffersWebsite.Models;
using WebApplication1.Models;

namespace JobOffersWebsite.Controllers
{
    public class Jobs1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs1
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.Category).Include(j => j.User);
            return View(jobs.ToList());
        }

        // GET: Jobs1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs1/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName");
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "UserType");
            return View();
        }

        // POST: Jobs1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobName,JobDescription,JobImage,CategoryID,UserID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName", job.CategoryID);
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "UserType", job.UserID);
            return View(job);
        }

        // GET: Jobs1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName", job.CategoryID);
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "UserType", job.UserID);
            return View(job);
        }

        // POST: Jobs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobName,JobDescription,JobImage,CategoryID,UserID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName", job.CategoryID);
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "UserType", job.UserID);
            return View(job);
        }

        // GET: Jobs1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
