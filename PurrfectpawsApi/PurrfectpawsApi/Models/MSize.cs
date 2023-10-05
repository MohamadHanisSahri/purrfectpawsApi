using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class MSize
{
    [Key]
    public int SizeId { get; set; }

    public string SizeLabel { get; set; } = null!;

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
