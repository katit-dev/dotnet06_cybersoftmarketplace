using System;
using System.Collections.Generic;

namespace dotnet06_CybersoftMarketPlace.Api;

public partial class Cart
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string? Alias { get; set; }

    public string? AdditionalData { get; set; }

    public bool? Deleted { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
