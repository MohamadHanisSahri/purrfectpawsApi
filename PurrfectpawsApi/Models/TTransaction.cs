using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurrfectpawsApi.Models;

public partial class TTransaction
{
    [Key]
    public int TransactionId { get; set; }

    public int PaymentStatusId { get; set; }

    public int OrderMasterId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TransactionAmount { get; set; }

    public  MOrderMaster OrderMaster { get; set; } = null!;

    public  MPaymentStatus PaymentStatus { get; set; } = null!;
}


public partial class TPostTransaction
{
    [Key]
    public int TransactionId { get; set; }

    public int PaymentStatusId { get; set; }

    public int OrderMasterId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TransactionAmount { get; set; }

}

public partial class GetTTransaction
{
    public int TransactionId { get; set; }

    public int PaymentStatusId { get; set; }

    public int OrderMasterId { get; set; }

    public DateTime TransactionDate { get; set; }

    public decimal TransactionAmount { get; set; }

    public TransactionOrderMaster TransactionOrderMaster { get; set; } = null!;
    public TransactionOrderStatus TransactionOrderStatus { get; set; } = null!;

    public MPaymentStatus PaymentStatus { get; set; } = null!;
}

public partial class TransactionOrderMaster
{
    public virtual ICollection<GetTOrder> TOrders { get; set; } = new List<GetTOrder>();

    public virtual TUserGetsDTO User { get; set; } = null!;

}
public partial class TransactionOrderStatus
{
    public string Status { get; set; } = null!;
}

