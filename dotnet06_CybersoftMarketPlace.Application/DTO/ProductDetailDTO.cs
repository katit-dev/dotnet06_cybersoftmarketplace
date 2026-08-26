
public class ProductDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ratingScore { get; set; } = 0;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public List<string> ListImageUrl { get; set; } = new List<string>();
    public List<ProductVariantDetailDTO> ListProductVariant { get; set; } = new List<ProductVariantDetailDTO>();
    public ShopProductDetailDTO shopProductDetailDTO { get; set; } = new ShopProductDetailDTO();

}


public class ProductVariantDetailDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int Stock { get; set; } = 0;
}

public class ShopProductDetailDTO
{
    public int Id { get; set; }
    public string ShopName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Image { get; set; } = null!;
}

