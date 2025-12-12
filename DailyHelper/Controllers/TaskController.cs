using System;
using System.Linq;
using System.Web.Mvc;
using DailyHelper.Models;
using DailyHelper.Data;
using System.Data.Entity;

namespace DailyHelper.Controllers
{
    public class TaskController : Controller
    {
        private readonly DailyHelperContext db = new DailyHelperContext();

        // Middleware check: Users must be logged in
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] == null)
                filterContext.Result = RedirectToAction("Login", "Account");
            base.OnActionExecuting(filterContext);
        }

        // User Dashboard (READ operation)
        public ActionResult Index()
        {
            int uid = (int)Session["UserID"];
            var tasks = db.Tasks
                        .Where(t => t.UserID == uid) // Only user's own tasks
                        .OrderBy(t => t.DueDate)
                        .ToList();

            ViewBag.Announcements = db.Announcements.OrderByDescending(a => a.DatePosted).ToList();
            return View(tasks);
        }

        // Calendar View
        public ActionResult Calendar()
        {
            int uid = (int)Session["UserID"];
            var tasks = db.Tasks.Where(t => t.UserID == uid).ToList();

            ViewBag.TodayTasks = tasks.Where(x => x.DueDate.Date == DateTime.Today).ToList();
            return View(tasks);
        }

        // CREATE operation - GET
        public ActionResult Create() => View();

        // CREATE operation - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.UserID = (int)Session["UserID"];
                task.Status = "Pending";

                db.Tasks.Add(task);
                db.SaveChanges(); // SAVE Task

                return RedirectToAction("Index");
            }
            return View(task);
        }

        // UPDATE operation - GET
        public ActionResult Edit(int id)
        {
            var task = db.Tasks.Find(id);
            if (task == null || task.UserID != (int)Session["UserID"]) return RedirectToAction("Index");
            return View(task);
        }

        // UPDATE operation - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskItem task)
        {
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges(); // UPDATE Task
            return RedirectToAction("Index");
        }

        // DELETE operation
        public ActionResult Delete(int id)
        {
            var task = db.Tasks.Find(id);
            if (task != null && task.UserID == (int)Session["UserID"])
            {
                db.Tasks.Remove(task);
                db.SaveChanges(); // DELETE Task
            }
            return RedirectToAction("Index");
        }
    }
}