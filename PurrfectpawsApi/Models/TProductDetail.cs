using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class TProductDetail
{
    [Key]
    public int ProductDetailsId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal? ProductCost { get; set; }

    public decimal? ProductRevenue { get; set; }

    public decimal? ProductProfit { get; set; }

    public int? QuantitySold { get; set; }

    public virtual MCategory Category { get; set; } = null!;

    public virtual ICollection<TProductBlobImage> TProductBlobImages { get; set; } = new List<TProductBlobImage>();

    public virtual ICollection<TProduct> TProducts { get; set; } = new List<TProduct>();
}

public partial class TPostProductDetail
{
    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal? ProductCost { get; set; }

    public decimal? ProductRevenue { get; set; }

    public decimal? ProductProfit { get; set; }

    public int? QuantitySold { get; set; }

    public List<IFormFile> Images { get; set; }

}


public partial class TPutProductDetail
{
    [Key]
    public int ProductDetailsId { get; set; }
    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal? ProductCost { get; set; }

    public decimal? ProductRevenue { get; set; }

    public decimal? ProductProfit { get; set; }

    public int? QuantitySold { get; set; }

    public List<IFormFile> Images { get; set; }

}


