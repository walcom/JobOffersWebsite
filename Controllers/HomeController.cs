using JobOffersWebsite.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            //var list = db.Categories.ToList();
            //return View(list);
            return View(db.Categories.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }

            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }

        [Authorize]
        public ActionResult Apply(int id)
        {
            Session["JobId"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult Apply(int id, string Message)
        {
            var userID = User.Identity.GetUserId();
            //var jobID = (int)Session["JobId"];

            var check = db.ApplyForJob.Where(a => a.JobId == id && a.UserId == userID).ToList();
            if (check.Count() < 1)
            {
                var job = new ApplyForJob();
                job.UserId = userID;
                job.JobId = id; // jobID;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJob.Add(job);
                db.SaveChanges();

                ViewBag.Result = "تم التقدم بنجاح";
            }
            else
                ViewBag.Result = "المعذرة، لقد سبق وتقدمت إلى نفس الوظيفة";

            return View();
            //return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var userID = User.Identity.GetUserId();
            var jobs = db.ApplyForJob.Where(a => a.UserId == userID);

            return View(jobs.ToList());
        }

        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var userID = User.Identity.GetUserId();
            var jobs = from app in db.ApplyForJob
                       join job in db.Jobs on app.JobId equals job.Id
                       where job.User.Id == userID
                       select app;

            var grouped = from j in jobs
                          group j by j.job.JobName into gr
                          select new JobsViewModel { JobTitle = gr.Key, Items = gr };

            //return View(jobs.ToList());
            return View(grouped.ToList());
        }


        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplyForJob.Find(id);
            if (job == null)
                return HttpNotFound();

            return View(job);
        }



        public ActionResult Edit(int id)
        {
            var j = db.ApplyForJob.Find(id);
            if (j == null)
                return HttpNotFound();

            return View(j);
        }

        [HttpPost]
        public ActionResult Edit(ApplyForJob j)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    j.ApplyDate = DateTime.Now;
                    db.Entry(j).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GetJobsByUser");
                }

                return View(j);
            }
            catch
            {
                return View(j);
            }
        }

        public ActionResult Delete(int id)
        {
            var j = db.ApplyForJob.Find(id);
            if (j == null)
                return HttpNotFound();

            return View(j);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(Job j)
        {           
            var r = db.ApplyForJob.Find(j.Id);
            db.ApplyForJob.Remove(r);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");          
        }


        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

    }
}