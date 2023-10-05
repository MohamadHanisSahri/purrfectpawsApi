using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurrfectpawsApi.Models;

public partial class TUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? AccessToken { get; set; }
    [ForeignKey("RoleId")]
    public MRole Role { get; set; }

    public virtual ICollection<MOrderMaster> MOrderMasters { get; set; } = new List<MOrderMaster>();

    public virtual ICollection<TBillingAddress> TBillingAddresses { get; set; } = new List<TBillingAddress>();

    public virtual ICollection<TCart> TCarts { get; set; } = new List<TCart>();

    public virtual ICollection<TShippingAddress> TShippingAddresses { get; set; } = new List<TShippingAddress>();
}

public partial class TUserGetsDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string Role { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public List<TShippingAddress> ShippingAddress { get; set; }
    public List<TBillingAddress> BillingAddresses { get; set; }
}

public class TUserDTO
{
    public string role { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public bool isBillingAddressSame { get; set; }
    public string street_1 { get; set; }
    public string? street_2 { get; set;}
    public string city { get; set; }
    public string state { get; set; }
    public int postcode { get; set; }
    public string? billingStreet_1 { get; set; }
    public string? billingStreet_2 { get; set; }
    public string? billingCity { get; set; }
    public string? billingState { get; set; }
    public int? billingPostcode { get; set; }

}

public class TUserPutDto
{
    public string name { get; set; }
    public string email { get; set; }
    public bool isBillingAddressSame { get; set; }
    public string street_1 { get; set; }
    public string? street_2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public int postcode { get; set; }
    public string? billingStreet_1 { get; set; }
    public string? billingStreet_2 { get; set; }
    public string? billingCity { get; set; }
    public string? billingState { get; set; }
    public int? billingPostcode { get; set; }
}

public class TUserLogin
{
    public string email { get; set; }

    public string Password { get; set; }

}

public class TUserPasswordPutDto
{
    public string oldPassword { get; set; }

    public string newPassword { get; set; }
}

//public class TUserGetAllDTO
//{

//}
