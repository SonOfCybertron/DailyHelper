using System.Linq;
using System.Web.Mvc;
using DailyHelper.Models;
using DailyHelper.Data;
using DailyHelper.Helper;

namespace DailyHelper.Controllers
{
    public class AccountController : Controller
    {
        DailyHelperContext db = new DailyHelperContext();

        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string hash = SecurityHelper.HashPassword(password);

            // EF LINQ Query: Checks credentials against hash in DB
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == hash);

            if (user != null)
            {
                // Start Session
                Session["UserID"] = user.UserID;
                Session["Username"] = user.Username;
                Session["Role"] = user.Role;

                if (user.Role == "Admin") return RedirectToAction("Dashboard", "Admin");
                return RedirectToAction("Index", "Task");
            }

            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        public ActionResult Register() => View();

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Username == user.Username))
                {
                    ViewBag.Error = "Username already taken.";
                    return View(user);
                }

                user.Password = SecurityHelper.HashPassword(user.Password);
                user.Role = "User"; // Default role

                db.Users.Add(user);
                db.SaveChanges(); // Saves new user to database

                return RedirectToAction("Login");
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}