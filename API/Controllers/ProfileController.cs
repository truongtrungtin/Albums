using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Library;
using Infrastructure.Data.Models;
using Infrastructure.Data.Responses;
using Infrastructure.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {
        private readonly IRepository<ProfileModel> _profileRepository;
        private Guid? _currentUser;

        public ProfileController(
        IRepository<ProfileModel> profileRepository,
        IRepository<CatalogModel> catalogRepository,
        Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment
        )
        {
            _profileRepository = profileRepository;
            _catalogRepository = catalogRepository;
            _hostingEnvironment = hostingEnvironment;

        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var profiles = await _profileRepository.ListAsync();
            var data = new List<ProfileViewModel>();
            foreach (var item in profiles)
            {
                data.Add(new ProfileViewModel()
                {
                    ProfileId = item.ProfileId,
                    ProfileCode = item.ProfileCode,
                    ProfileName = item.FirstName,
                    Avatar = item.Avatar,
                    CreateBy = item.CreateBy,   
                    Actived = item.Actived,
                });
            }
            return Ok(data);
        }


        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> Post([FromForm] CreateProfileViewModel createProfile)
        {
            try
            {
                _currentUser = Guid.Parse(User?.FindFirstValue(ClaimTypes.NameIdentifier));

                var data = new ProfileModel()
                {
                    ProfileId= Guid.NewGuid(),
                    FirstName = createProfile.ProfileName,
                    CreateTime = DateTime.Now,
                    CreateBy= _currentUser,
                    Actived = true
                };
                data.Avatar = await new UploadFilesLibrary(_hostingEnvironment, new JwtSettings())
               .UploadFile(createProfile.Avatar, "FileAttachments/"+_currentUser+"/"+data.ProfileId);

                await _profileRepository.AddAsync(data);
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

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
