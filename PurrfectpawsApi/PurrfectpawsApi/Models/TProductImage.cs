using System;
using System.Collections.Generic;

namespace PurrfectpawsApi.Models;

public partial class TProductImage
{
    public int ImageId { get; set; }

    public int ProductDetailsId { get; set; }

    public string BlobStorageId { get; set; } = null!;
}
