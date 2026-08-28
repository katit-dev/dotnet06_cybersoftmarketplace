


using System.Collections.Generic;
using System.Linq;
using backend_netcore_dotnet06.Helper;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
public interface IProductService
{
    Task<HTTPResponseData<List<ProductIndexPageDTO>>> GetAllProductsAsync(string keyword = "", int pageIndex = 1, int pageSize = 10);

    Task<HTTPResponseData<ProductDetailDTO>> GetProductDetailAsync(int productId);} 

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<HTTPResponseData<List<ProductIndexPageDTO>>> GetAllProductsAsync(string keyword = "", int pageIndex = 1, int pageSize = 10)
    {
        //Lấy danh sách sản phẩm
        //Lấy repository từ UnitOfWork
        IProductRepository? productsRepo = _unitOfWork.ProductRepository;

        keyword = HelperFunction.StringToSlug(keyword);
        var products = await productsRepo.Where(p=>p.Deleted == false && p.Alias.Contains(keyword));

        Console.WriteLine($"Số lượng sản phẩm: {products.Count()}");

        //Phân trang
        if (products == null || products.Count() == 0)
        {
            return new HTTPResponseData<List<ProductIndexPageDTO>>
            {
                DataResponse = new List<ProductIndexPageDTO>(),
                Message = "Không có sản phẩm nào",
                statusCode = 200
            };
        }
        
        var data = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => new ProductIndexPageDTO
        {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.ProductImages.FirstOrDefault().ImageUrl ?? "https://via.placeholder.com/150",
            ProductCategory = new ProductCategoryIndexPageDTO
            {
                Id = p.Category.Id,
                Name = p.Category.Name
            },
            Shop = new ShopProductIndexPageDTO
            {
                Id = p.Shop.Id,
                Name = p.Shop.ShopName,
                Description = p.Shop.Description ?? ""
            },
            Price = p.ProductVariants.FirstOrDefault().Price,
        }).ToList();

        return new HTTPResponseData<List<ProductIndexPageDTO>>
        {
            DataResponse = data,
            Message = "Lấy danh sách sản phẩm thành công",
            statusCode = 200
        };



        
    }

    public async Task<HTTPResponseData<ProductDetailDTO>> GetProductDetailAsync(int productId)
    {
        ProductDetailDTO? prodDetailDTO = new ProductDetailDTO();
        //process:
        //Cách 1 dùng singleordefault thực hiện fill từng trường bằng nhiều repository khác nhau
        //1. Lấy sản phẩm từ repository  
        // Product? prodDetail = await _unitOfWork.ProductRepository.SingleOrDefault(prod => prod.Id == productId && prod.Deleted == false);
        // if(prodDetail == null)
        // {
        //     return new HTTPResponseData<ProductDetailDTO>
        //     {
        //         DataResponse = null,
        //         Message = "Không tìm thấy sản phẩm",
        //         statusCode = 404
        //     };
        // }
        // //Map dữ liệu từ bảng product vào dto  
        // prodDetailDTO.Id = prodDetail.Id;
        // prodDetailDTO.Name = prodDetail.Name;
        // prodDetailDTO.Description = prodDetail.Description ?? "";
        // prodDetailDTO.Price = prodDetail.ProductVariants.FirstOrDefault()?.Price ?? 0;
        // prodDetailDTO.ImageUrl = prodDetail.Image ?? "https://via.placeholder.com/150";

        // //Lấy danh sách hình ảnh của sản phẩm từ bảng ProductImage
        // prodDetailDTO.ListImageUrl = _unitOfWork.ProductImageRepository.Where(prodImg => prodImg.ProductId == productId).Result.Select(prodImg => prodImg.ImageUrl).ToList();


        // //Lấy danh sách biến thể của sản phẩm từ bảng ProductVariant
        // prodDetailDTO.ListProductVariant = _unitOfWork.ProductVariantRepository.Where(prodVar => prodVar.ProductId == productId).Result.Select(prodVar => new ProductVariantDetailDTO
        // {
        //     Id = prodVar.Id,
        //     Name = prodVar.VariantName,
        //     Price = prodVar.Price,
        //     ImageUrl = prodVar.Image ?? "https://via.placeholder.com/150",
        //     Stock = prodVar.Stock
        // }).ToList();

        // //Map thông tin shop vào dto productdetailDTO
        // prodDetailDTO.shopProductDetailDTO = _unitOfWork.ShopRepository.SingleOrDefault(shop => shop.Id == prodDetail.ShopId).Result is Shop shop ? new ShopProductDetailDTO
        // {
        //     Id = prodDetail.Shop.Id,
        //     ShopName = prodDetail.Shop.ShopName,
        //     Description = prodDetail.Shop.Description ?? "",
        //     Image = prodDetail.Shop.Image ?? "https://via.placeholder.com/150"
        // } : new ShopProductDetailDTO
        // {
        //     Id = 0,
        //     ShopName = "Không xác định",
        //     Description = "",
        //     Image = "https://via.placeholder.com/150"
        // };



        //Cách 2: Dùng 1 lệnh select từ repository gốc là product, join với các bảng liên quan để lấy dữ liệu, sau đó map vào dto. Cách này sẽ giảm số lần gọi database nhưng code sẽ phức tạp hơn.
        var prodDetailDTO2 =  _unitOfWork.ProductRepository.WhereSql(prod => prod.Id == productId && prod.Deleted == false).Select(prod => new ProductDetailDTO
        {
            Id = prod.Id,
            Name = prod.Name,
            Description = prod.Description ?? "",
            Price = prod.ProductVariants.FirstOrDefault().Price,
            ImageUrl = prod.Image ?? "https://via.placeholder.com/150",
            ListImageUrl = prod.ProductImages.Select(prodImg => prodImg.ImageUrl).ToList(),
            ListProductVariant = prod.ProductVariants.Select(prodVar => new ProductVariantDetailDTO
            {
                Id = prodVar.Id,
                Name = prodVar.VariantName,
                Price = prodVar.Price,
                ImageUrl = prodVar.Image ?? "https://via.placeholder.com/150",
                Stock = prodVar.Stock
            }).ToList(),
            shopProductDetailDTO = new ShopProductDetailDTO
            {
                Id = prod.Shop.Id,
                ShopName = prod.Shop.ShopName,
                Description = prod.Shop.Description ?? "",
                Image = prod.Shop.Image ?? "https://via.placeholder.com/150"
            }
        }).FirstOrDefault();
        

        return new HTTPResponseData<ProductDetailDTO> //return response
        {
            DataResponse = prodDetailDTO2,
            Message = "Lấy chi tiết sản phẩm thành công",
            statusCode = 200
        };

    }

}