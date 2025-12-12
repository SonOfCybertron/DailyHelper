using System.Data.Entity;
using DailyHelper.Models;

namespace DailyHelper.Data
{
    public class DailyHelperContext : DbContext
    {
        public DailyHelperContext() : base("name=DailyHelperContext")
        {
            Database.SetInitializer<DailyHelperContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
    }
}