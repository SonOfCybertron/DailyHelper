using System.Linq;
using System.Web.Mvc;
using DailyHelper.Models;
using DailyHelper.Data;
using System.Data.Entity;

namespace DailyHelper.Controllers
{
    public class AdminController : Controller
    {
        private readonly DailyHelperContext db = new DailyHelperContext();

        // Authorization check: Must be "Admin" role
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Role"]?.ToString() != "Admin")
                filterContext.Result = RedirectToAction("Login", "Account");
            base.OnActionExecuting(filterContext);
        }

        // Admin Dashboard (Monitor Tasks, User Dashboard)
        public ActionResult Dashboard()
        {
            // Fetch ALL tasks and INCLUDE the User details (for monitoring)
            var allTasks = db.Tasks.Include(t => t.User).OrderByDescending(t => t.DueDate).ToList();

            ViewBag.TotalTasks = allTasks.Count;
            ViewBag.TotalUsers = db.Users.Count();

            return View(allTasks);
        }

        // Announcement Feature (CREATE)
        [HttpPost]
        public ActionResult CreateAnnouncement(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Announcement ann = new Announcement { Message = message };
                db.Announcements.Add(ann);
                db.SaveChanges(); // SAVE Announcement
            }
            return RedirectToAction("Dashboard");
        }
    }
}