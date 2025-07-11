using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillaAPI.Models;

public class Villa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Details { get; set; }
    [Required]
    public required string Amenity { get; set; }
    public string? ImageURL { get; set; }
    [Required]
    public double Rate { get; set; }
    [Required]
    public int Occupancy { get; set; }
    [Required]
    public double Sqft { get; set; }
}
