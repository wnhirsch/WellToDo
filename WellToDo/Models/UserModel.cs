using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WellToDo.Models;

[Table("User", Schema = "dbo")]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = "";

    [Required]
    [MaxLength(50)]
    public string Firstname { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Lastname { get; set; } = "";

    public static implicit operator int(User v)
    {
        throw new NotImplementedException();
    }
}

public class UserLoginRequest
{
    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = "";
}

public class UserSignUpRequest
{
    [Required]
    [MaxLength(20)]
    public string Username { get; set; } = "";

    [Required]
    [MaxLength(50)]
    public string Firstname { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Lastname { get; set; } = "";
}