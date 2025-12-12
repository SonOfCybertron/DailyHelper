using System.Web.Mvc;
using System.Web.Routing;

namespace DailyHelper
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            // Admin routes - specific routes should come before default routes
            routes.MapRoute(
                name: "AdminDashboard",
                url: "Admin/Dashboard",
                defaults: new { controller = "Admin", action = "Dashboard" }
            );

            // Task routes
            routes.MapRoute(
                name: "TaskCalendar",
                url: "Task/Calendar",
                defaults: new { controller = "Task", action = "Calendar" }
            );

            routes.MapRoute(
                name: "TaskDetail",
                url: "Task/{action}/{id}",
                defaults: new { controller = "Task" },
                constraints: new { id = @"\d+" }  // ID must be numeric
            );

            // Account routes
            routes.MapRoute(
                name: "AccountActions",
                url: "Account/{action}",
                defaults: new { controller = "Account", action = "Login" }
            );

            // Default route - must be last
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                constraints: new { id = @"\d*" }  // Optional numeric ID
            );
        }
    }
}