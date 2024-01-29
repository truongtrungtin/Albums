using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Models;
using Infrastructure.Data.Responses;
using Infrastructure.Data.Specifications;
using Infrastructure.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data.Library;
using StackExchange.Redis;
using System.Security.Claims;

namespace API.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : BaseController
{
    private readonly IRepository<ProductModel> _productRepository;
    private Guid? _currentUser;

    public ProductsController(
        Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment,
        IRepository<ProductModel> productRepository)
    {
        _productRepository = productRepository;
        _hostingEnvironment = hostingEnvironment;
    }

    // POST: api/v1/products
    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProduct createProduct)
    {

        try
        {
            this._currentUser = Guid.Parse(User?.FindFirstValue(ClaimTypes.NameIdentifier));
            createProduct.File.Content = createProduct.File.Content.Replace('-', '+').Replace('_', '/');

            byte[] fileContent = Convert.FromBase64String(createProduct.File.Content.Trim());


            // Create an in-memory IFormFile
            var file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "file", createProduct.File.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = createProduct.File.Type
            };
            // Map DTO to the Product entity[[
            //foreach (var item in files)
            //{
            var product = new ProductModel()
            {
                ProductId = Guid.NewGuid(),
                ProductName = createProduct.File.Name,
                UPC = createProduct.File.Type,
                Quantity = createProduct.File.Size,
                CreateBy = _currentUser,
                CreateTime = DateTime.UtcNow,
                
            };
            product.Image = await new UploadFilesLibrary(_hostingEnvironment, new JwtSettings())
                .UploadFile(file, "Product");

            await _productRepository.AddAsync(product);

            //}


            // Return the created product
            return Ok(new ApiResponse()
            {
                code = 200,
                isSuccess = true,
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message,
            });
        }
    }

    //[HttpPost("types")]
    ////[Authorize(Roles = "Admin")] // Add authorization if needed
    //public async Task<ActionResult> CreateProductType([FromBody] ProductTypeCreateDTO productTypeCreateDTO)
    //{
    //    try
    //    {
    //        // Map DTO to the ProductType entity
    //        var productType = _mapper.Map<ProductCategoryModel>(productTypeCreateDTO);

    //        // Add additional logic if needed, such as validation or processing

    //        // Save the product type to the repository
    //        await _productCategoryRepository.AddAsync(productType);
    //        // Map the created product type back to DTO for the response
    //        var productTypeDTO = _mapper.Map<ProductTypeCreateDTO>(productType);

    //        // Return the created product type
    //        return Ok(new
    //        {
    //            Message = 200,
    //            Data = productType
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the exception details here
    //        return StatusCode(500, ex.Data);
    //    }
    //}


    // GET: api/v1/products
    //[HttpGet]
    //public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductParams productParams)
    //{
    //    try
    //    {
    //        // Extract parameters from productParams
    //        var sort = productParams.Sort;
    //        var productTypeId = productParams.ProductTypeId;
    //        var productBrandId = productParams.ProductBrandId;
    //        var search = productParams.Search;
    //        var page = productParams.Page;
    //        var pageSize = productParams.PageSize;
    //        var skip = productParams.GetSkip(); // Assuming GetSkip is a method to calculate the number of items to skip

    //        // Specification for counting
    //        var countSpec = new ProductCountSpecification(sort, productTypeId, productBrandId, search);
    //        var totalItems = await _productRepository.CountAsync(countSpec);
    //        if (totalItems == 0)
    //        {
    //            return Ok(new Pagination<ProductDTO>(page, 0, pageSize, totalItems, new List<ProductDTO>()));
    //        }
    //        // Specification for paginated data
    //        var spec = new ProductWithTypesAndBrandSpecification(sort, productTypeId, productBrandId, skip, pageSize, search);
    //        var products = await _productRepository.ListAsync(spec);
    //        var productDTOs = _mapper.Map<List<ProductDTO>>(products);

    //        // Create and return the paginated result
    //        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
    //        var pagination = new Pagination<ProductDTO>(page, totalPages, pageSize, totalItems, productDTOs);
    //        return Ok(pagination);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log the exception details here
    //        return StatusCode(500, "An error occurred while processing your request.");
    //    }
    //}

    [HttpGet]
    public async Task<ActionResult> Search([FromQuery] ProductParams productParams)
    {
        try
        {
            // action inside a standard controller
            var sort = productParams.Sort;
            var type = productParams.ProductTypeId;
            var page = productParams.Page;
            var pageSize = productParams.PageSize;
            var skip = productParams.GetSkip();

            var countSpec = new ProductCountSpecification(sort, type);
            var totalItems = await _productRepository.CountAsync(countSpec);
            if (totalItems == 0)
            {
                return Ok(new Pagination<ProductViewModel>(page, 0, pageSize, totalItems, new List<ProductViewModel>()));
            }

            var spec = new ProductWithTypesAndBrandSpecification(sort, type, skip, pageSize);
            var products = await _productRepository.ListAsync(spec);
            var data = new List<ProductViewModel> { };

            if (products != null && products.Count() > 0)
            {
                foreach (var item in products)
                {

                    #region Images
                    item.Image = Path.Combine(_hostingEnvironment.ContentRootPath, "Upload/" + item.Image);
                    //item.Image = images;

                    //var imgs = _context.FileAttachmentModel.Where(x => x.ObjectId == item.Id).ToList();
                    //if (imgs.Count() > 0)
                    //{
                    //    item.Image = imgs.FirstOrDefault().FileUrl;

                    //}
                    //else
                    //{
                    //    item.Image = "/Images/product.jfif";
                    //}
                    #endregion
                    data.Add(new ProductViewModel
                    {
                        ProductId = item.ProductId,
                        Name = item.ProductName,
                        url = item.Image,
                        productType = item.UPC,
                        size = item.Quantity
                    });
                }

            }

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagination = new Pagination<ProductViewModel>(page, totalPages, pageSize, totalItems, data);

            return Ok(new ApiResponse()
            {
                code = 200,
                isSuccess = true,
                data = pagination,
            });
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }

    }


    // GET: api/v1/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult> GetProduct(Guid id)
    {
        var spec = new ProductWithTypesAndBrandSpecification(id);
        var product = await _productRepository.GetByIdAsync(spec);
        return Ok(product);
    }

    //[HttpGet("types")]
    //public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetProductTypes()
    //{
    //    return Ok(await _productCategoryRepository.ListAsync());
    //}
}