using System;
using System.Collections.Generic;

namespace PurrfectpawsApi.Models;

public partial class TProductImagesDto
{
    public int ImagesId { get; set; }
    public string BlobImageUrl { get; set; }
}
public partial class TProductsDto
{
    public int ProductId { get; set; }
    public int ProductDetailsId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductCategoryId { get; set; }
    public string ProductCategory { get; set; }
    public int StockQuantity { get; set; }
    public string ProductVariation { get; set; }
    public string? ProductSize { get; set; }
    public decimal? ProductLength { get; set; }
    public List<TProductImagesDto> ProductImages { get; set; }

}