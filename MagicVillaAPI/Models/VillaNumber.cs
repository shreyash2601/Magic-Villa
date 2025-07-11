using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MagicVillaAPI.Models;

public class VillaNumber
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VillaNo { get; set; }
    [ForeignKey("Villa")]
    public int VillaId { get; set; }
     [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Villa? Villa { get; set; }
    public string? SpecialDetails { get; set; }
}
