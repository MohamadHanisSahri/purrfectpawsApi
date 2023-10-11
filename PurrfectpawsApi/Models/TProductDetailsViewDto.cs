using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models;

public partial class SizeDetailsDto
{
    public int SizeId { get; set; }
    public string SizeLabel { get; set; }
    public int ProductQuantity { get; set; }
}
public partial class LeadLengthDetailsDto
{
    public int LeadLengthId { get; set; }
    public decimal LeadLength { get; set; }
    public int ProductQuantity { get; set; }
}
public partial class VariationDto
{
    public int VariationId { get; set; }

    public string VariationName { get; set; } = null!;
    public int ProductQuantity { get; set; }
}
public partial class ImageDetailsDto
{
    public int ProductImageId { get; set; }
    public int ProductDetailsId { get; set; }
    public string BlobStorageId { get; set; }
}
public partial class TProductDetailsViewDto
{
    public int ProductId { get; set; }
    public int ProductDetailsId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public string ProductImages { get; set; }
    public List<ImageDetailsDto> Images { get; set; }
    public List<SizeDetailsDto> Sizes { get; set; }
    public List<LeadLengthDetailsDto> LeadLengths { get; set; }
    public List<VariationDto> Variations { get; set; }
}

public partial class TProductDetailsQuantityDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public partial class TDetailsDto
{
    public int? ProductId { get; set; }
    public int? SizeId { get; set; } = null;
    public string? SizeLabel { get; set; }
    public int? LeadLengthId { get; set; } = null;
    public decimal? LeadLength { get; set; }
    public int? VariationId { get; set; } = null;

    public string VariationName { get; set; }
    public int ProductQuantity { get; set; }

}

public partial class TProductListByProductDetailsIdDto
{
    public int ProductDetailsId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public List<ImageDetailsDto> Images { get; set; }
    public List<TDetailsDto> TDetails { get; set; }

}

public partial class TUpdateProductDetailsDto
{
    public int ProductDetailsId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public List<TDetailsDto> TDetails { get; set; }

}