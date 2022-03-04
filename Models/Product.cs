using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetMvc.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public Decimal Price { get; set; }
}