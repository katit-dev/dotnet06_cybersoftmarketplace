public class CartDTO
{
    public string UserId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now; //Ngày tạo giỏ hàng
    public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>(); //Danh sách các sản phẩm variant trong giỏ hàng
}


public class CartItemDTO
{
    public int ProductVariantId { get; set; } //Id của sản phẩm variant
    public int Quantity { get; set; } //Số lượng sản phẩm variant trong giỏ hàng
    public decimal Price { get; set; } //Giá của sản phẩm variant
    public string Image { get; set; } = string.Empty; //Hình ảnh của sản phẩm variant
    public string Name { get; set; } = string.Empty; //Tên của sản phẩm variant
}