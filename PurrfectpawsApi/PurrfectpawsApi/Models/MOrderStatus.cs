using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class MOrderStatus
{
    [Key]
    public int OrderStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
