using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Dtos;

public class UpdateUserDto
{
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Range(0, 130)]
    public int? Age { get; set; }
}
