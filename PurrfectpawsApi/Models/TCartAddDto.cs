using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurrfectpawsApi.Models;

public partial class TCartAddDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Quantity { get; set; }

}
