using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class MOrderMaster
{
    [Key]
    public int OrderMasterId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    public virtual ICollection<TTransaction> TTransactions { get; set; } = new List<TTransaction>();

    public virtual TUser User { get; set; } = null!;
}
