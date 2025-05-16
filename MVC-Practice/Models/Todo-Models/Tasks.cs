using System.ComponentModel.DataAnnotations;

namespace MVC_Practice.Models
{
    public class Tasks
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(4000, ErrorMessage = "Too long title")]
        public string Title { get; set; } = string.Empty;
        
        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedDate { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;

    }
}
