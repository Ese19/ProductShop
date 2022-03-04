using System.ComponentModel.DataAnnotations;

namespace NetMvc.Models;

public class CreateRole
{
    [Required]
    public string? RoleName { get; set; }
}