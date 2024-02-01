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
using Microsoft.VisualBasic.FileIO;

namespace API.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class FileAttachmentController : BaseController
{
    private readonly IRepository<FileAttachmentModel> _fileAttachmentRepository;

    private Guid _currentUser;

    public FileAttachmentController(
        IRepository<FileAttachmentModel> fileAttachmentRepository,
        IRepository<CatalogModel> catalogRepository,
        Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment

        )
    {
        _fileAttachmentRepository = fileAttachmentRepository;
        _catalogRepository = catalogRepository;
        _hostingEnvironment = hostingEnvironment;

    }

    // POST: api/v1/products
    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<ActionResult> Add([FromForm] CreateFileAttachment createFile)
    {

        try
        {
            _currentUser = Guid.Parse(User?.FindFirstValue(ClaimTypes.NameIdentifier));

            var profile = new ProfileModel();


            var file = new FileAttachmentModel()
            {
                FileAttachmentId = Guid.NewGuid(),
                FileAttachmentCode = _unitOfWork.UtilitiesRepository.GetFileTypeByExtension(createFile.File.ContentType),
                FileAttachmentName = createFile.File.FileName,
                FileType = createFile.File.ContentType,
                Size = createFile.File.Length,
                Latitude = !string.IsNullOrEmpty(createFile.Latitude) ? createFile.Latitude : null,
                Longitude = !string.IsNullOrEmpty(createFile.Longitude) ? createFile.Longitude : null,
                FileExtention = _unitOfWork.UtilitiesRepository.FileExtension(createFile.File.FileName),
                CreateBy = _currentUser,
                CreateTime = DateTime.UtcNow,

            };
            if (createFile.ProfileId.HasValue && Guid.Empty != createFile.ProfileId)
            {
                profile = _context.ProfileModel.FirstOrDefault(x => x.ProfileId == createFile.ProfileId);
                if (profile != null)
                {
                    file.ObjectId = profile.ProfileId;

                }
            }
            file.FileUrl = await new UploadFilesLibrary(_hostingEnvironment, new JwtSettings())
                .UploadFile(createFile.File, "FileAttachments/" + _currentUser + "/" + (profile != null ?  + profile.ProfileCode : 0));

            await _fileAttachmentRepository.AddAsync(file);


            // Return the created product
            return Ok(new ApiResponse()
            {
                code = 200,
                isSuccess = true,
                message = "Create success!"
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
    public async Task<ActionResult> List([FromQuery] FileAttachmentParams fileAttachmentParams)
    {
        try
        {
            _currentUser = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // action inside a standard controller
            var sort = fileAttachmentParams.Sort;
            var search = fileAttachmentParams.Search;
            var fileExtention = fileAttachmentParams.FileExtention;
            var fileType = fileAttachmentParams.FileType;
            var profile = fileAttachmentParams.profile;
            var page = fileAttachmentParams.Page;
            var pageSize = fileAttachmentParams.PageSize;
            var skip = fileAttachmentParams.GetSkip();

            var countSpec = new FileAttachmentCountSpecification(_currentUser, fileExtention, fileType, profile);
            var totalItems = await _fileAttachmentRepository.CountAsync(countSpec);
            if (totalItems == 0)
            {
                return Ok(new Pagination<FileAttachmentViewModel>(page, 0, pageSize, totalItems, new List<FileAttachmentViewModel>()));
            }

            var spec = new FileAttachmentModelWithExtentionAndTypeSpecification(_currentUser, sort, fileExtention, fileType,profile, skip, pageSize);
            var files = await _fileAttachmentRepository.ListAsync(spec);
            var data = new List<FileAttachmentViewModel> { };

            if (files != null && files.Count() > 0)
            {
                foreach (var item in files)
                {

                    #region Url
                    item.FileUrl = Path.Combine("/Upload/FileAttachments/" + _currentUser + "/" + (item.ObjectId != null && item.ObjectId != Guid.Empty ? item.ObjectId + "/" : "0/" + item.FileUrl));
                    #endregion
                    data.Add(new FileAttachmentViewModel
                    {
                        FileAttachmentId = item.FileAttachmentId,
                        FileAttachmentName = item.FileAttachmentName,
                        FileUrl = item.FileUrl,
                        FileExtention = item.FileExtention,
                        FileType = item.FileType,
                        Size = item.Size,
                    });
                }

            }

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagination = new Pagination<FileAttachmentViewModel>(page, totalPages, pageSize, totalItems, data);

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
        var spec = new FileAttachmentModelWithExtentionAndTypeSpecification(id);
        var product = await _fileAttachmentRepository.GetByIdAsync(spec);
        return Ok(product);
    }

    [HttpGet("fileextentions")]
    public async Task<ActionResult<IEnumerable<CatalogModel>>> GetFileExtentions()
    {
        var spec = new CatalogModelSpecification("FileExtentions");

        return Ok(await _catalogRepository.ListAsync(spec));
    }

    [HttpGet("filetypes")]
    public async Task<ActionResult<IEnumerable<CatalogModel>>> GetFileTypes()
    {
        var spec = new CatalogModelSpecification("FileTypes");

        return Ok(await _catalogRepository.ListAsync(spec));
    }
}