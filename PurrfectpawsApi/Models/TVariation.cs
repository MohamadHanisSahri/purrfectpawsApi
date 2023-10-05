using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class TVariation
{
    [Key]
    public int VariationId { get; set; }

    public string VariationName { get; set; } = null!;

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
