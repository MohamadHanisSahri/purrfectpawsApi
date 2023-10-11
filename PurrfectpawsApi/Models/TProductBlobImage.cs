using PurrfectpawsApi.Interfaces;
using System;
using System.Collections.Generic;

namespace PurrfectpawsApi.Models;

public partial class TProductBlobImage : ISoftDelete
{
    public int ProductImageId { get; set; }

    public int ProductDetailsId { get; set; }

    public string BlobStorageId { get; set; } = null!;

    public virtual TProductDetail ProductDetails { get; set; } = null!;

    public bool IsDeleted { get; set; }
}
