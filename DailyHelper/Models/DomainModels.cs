using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyHelper.Models
{
    [Table("tbl_users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // Stores SHA-256 Hash

        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("tbl_task")]
    public class TaskItem
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        // Setting format for display/editing
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        public string Status { get; set; } = "Pending";

        public int UserID { get; set; }

        // Navigation property for EF
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    [Table("tbl_announcement")]
    public class Announcement
    {
        [Key]
        public int AnnounceID { get; set; }
        public string Message { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}