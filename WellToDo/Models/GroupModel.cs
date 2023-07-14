using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellToDo.Models;

[Table("Group", Schema = "dbo")]
public class Group
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(7)]
    public string Color { get; set; } = "";

    [Required]
    public int UserId { get; set; }
}

public class GroupRequest
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(7)]
    public string Color { get; set; } = "";
}