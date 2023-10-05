using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class TProductImageDto
{
    public int ImageId { get; set; }
    public string ImageUrl { get; set; }
}

public partial class CartProductDetailsDto
{
    public int ProductId { get; set; }
    public int CartId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public string ProductSize { get; set; }
    public decimal ProductLength { get; set; }
    public string ProductVariation { get; set; }
    public int CartQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int StockQuantity { get; set; }
    public List<TProductImageDto> ProductImages { get; set; }

}

public partial class UserShippingAddressDto
{
    public int ShippingAdddressId { get; set; }
    public string? Street1 { get; set; }
    public string? Street2 { get; set; }
    public string? city { get; set; }
    public string? State { get; set; }
    public int? Postcode { get; set; }
    public string? Country { get; set; }

}

public partial class TCartListViewDto
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public UserShippingAddressDto UserShippingAddress { get; set; }
    public List<CartProductDetailsDto> ProductDetails { get; set; }
}



