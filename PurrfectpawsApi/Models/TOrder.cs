using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PurrfectpawsApi.Models;

public partial class TOrder
{
    [Key]
    public int OrderId { get; set; }

    public int OrderStatusId { get; set; }

    public int ProductId { get; set; }

    public int ShippingAddressId { get; set; }

    public int BillingAddressId { get; set; }

    public int OrderMasterId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual TBillingAddress? BillingAddress { get; set; }

    public virtual MOrderMaster OrderMaster { get; set; } = null!;

    public virtual MOrderStatus OrderStatus { get; set; } = null!;

    public virtual TProduct Product { get; set; } = null!;

    public virtual TShippingAddress ShippingAddress { get; set; } = null!;

}

public partial class TOrderDTO
{
    [Key]
    public int OrderId { get; set; }

    public int OrderStatusId { get; set; }

    public int ProductId { get; set; }

    public int ShippingAddressId { get; set; }

    public int BillingAddressId { get; set; }

    public int OrderMasterId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public TProduct? Product { get; set; }

    public int userID { get; set; }

}


public partial class TPutOrderDTO
{
    [Key]
    public int OrderId { get; set; }

    public int OrderStatusId { get; set; }

    public int ProductId { get; set; }

    public int ShippingAddressId { get; set; }

    public int BillingAddressId { get; set; }

    public int OrderMasterId { get; set; }

    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }


}


public partial class GetTOrder
{
    [Key]
    public int OrderId { get; set; }

    public int OrderStatusId { get; set; }

    public int ProductId { get; set; }

    public int ShippingAddressId { get; set; }

    public int BillingAddressId { get; set; }

    public int OrderMasterId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public MOrderStatus OrderStatus { get; set; }

    public MOrderMaster OrderMaster { get; set; }

    public TBillingAddress BillingAddress { get; set; }

    public TShippingAddress ShippingAddress { get; set; }

    public ProductOrderDetailsDTO ProductOrderDetailsDTO { get; set; } // Include a DTO for Product

}


public partial class ProductOrderDetailsDTO
{
    public TProductOrderDTO Product { get; set; }

    public TProductDetail ProductDetails { get; set; } // Include a DTO for ProductDetails

    public List<ImageDetailsDto> Images { get; set; }

}


public partial class TProductOrderDTO
{
    [Key]
    public int ProductId { get; set; }

    public int ProductQuantity { get; set; }

    public MSize? Size { get; set; }

    public TLeadLength? LeadLength { get; set; }

    public TVariation Variation { get; set; }


}

public partial class TPostOrderDTO
{

    public int OrderStatusId { get; set; }

   // public int ProductId { get; set; }

    public int ShippingAddressId { get; set; }

    public int BillingAddressId { get; set; }

    //public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public int UserId { get; set; }
    public int PaymentStatusId { get; set; }
    //public int CartId { get; set; }
  //  public decimal ShippingFee { get; set; }

}