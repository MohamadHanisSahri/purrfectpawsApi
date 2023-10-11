using PurrfectpawsApi.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;

namespace PurrfectpawsApi.Models;

public partial class TProduct : ISoftDelete
{
    [Key]
    public int ProductId { get; set; }

    [Column("product_details_id")]
    public int ProductDetailsId { get; set; }

    public int? SizeId { get; set; }

    public int? LeadLengthId { get; set; }

    public int VariationId { get; set; }

    public int ProductQuantity { get; set; }

    public int? QuantitySold { get; set; }

    public virtual TLeadLength? LeadLength { get; set; }

    public virtual TProductDetail ProductDetails { get; set; } = null!;

    public virtual MSize? Size { get; set; }

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    public virtual TVariation Variation { get; set; } = null!;

    public bool IsDeleted { get; set; }

}