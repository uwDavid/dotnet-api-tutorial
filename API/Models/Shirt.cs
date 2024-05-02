using System.ComponentModel.DataAnnotations;
using ApiDemo.Models.Validations;

namespace ApiDemo.Models;

public class Shirt
{
    public int ShirtId { get; set; }
    [Required]
    public string? Brand { get; set; }
    public string? Color { get; set; }
    [Shirt_EnsureCorrectSizing]
    public int? Size { get; set; }
    public string? Gender { get; set; }
    public double Price { get; set; }
}