using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurrfectpawsApi.Models;

public partial class TCart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Quantity { get; set; }

    public virtual TProduct Product { get; set; } = null!;

    public virtual ICollection<TTransaction> TTransactions { get; set; } = new List<TTransaction>();

    public virtual TUser User { get; set; } = null!;
}
