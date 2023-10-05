using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class TLeadLength
{
    [Key]
    public int LeadLengthId { get; set; }

    public decimal LeadLength { get; set; }

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}
