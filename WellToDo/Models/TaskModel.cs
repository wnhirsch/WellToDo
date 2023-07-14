using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellToDo.Models;

[Table("Task", Schema = "dbo")]
public class Task
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Title { get; set; } = "";

    [MaxLength(4000)]
    public string? Description { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public TaskPriority? Priority { get; set; }

    [MaxLength(2048)]
    public string? Url { get; set; }

    [Column(TypeName = "bit")]
    public bool IsChecked { get; set; }

    [Column(TypeName = "bit")]
    public bool IsFlagged { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    public int UserId { get; set; }
}

public enum TaskPriority : int
{
    Low = 1,
    Medium = 2,
    High = 3
}

public class TaskRequest
{
    [Required]
    [MaxLength(4000)]
    public string Title { get; set; } = "";

    [MaxLength(4000)]
    public string? Description { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public TaskPriority? Priority { get; set; }

    [MaxLength(2048)]
    public string? Url { get; set; }

    public bool IsChecked { get; set; }

    public bool IsFlagged { get; set; }

    [Required]
    public int GroupId { get; set; }
}

public class TaskFilterRequest
{
    public string? sentence { get; set; }

    public string? date { get; set; }

    public TaskPriority? priority { get; set; }

    public bool? isChecked { get; set; }

    public bool? isFlagged { get; set; }

    public int? groupId { get; set; }
}