using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetFileAPI.Resources.Input;
using NetFileAPI.Resources.Output;
using NetFileAPI.Services.Interfaces;

namespace NetFileAPI.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public FileController(ILogger<FileController> logger, IFileService fileService, IConfiguration configuration,
            IMapper mapper)
        {
            _logger = logger;
            _fileService = fileService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFiles()
        {
            var fileModels = await _fileService.GetFilesAsync();
            return Ok(fileModels);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFile([FromRoute] int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            return Ok(file);
        }


        [HttpGet]
        [Route("data/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFileData([FromRoute] int id)
        {
            var file = await _fileService.GetFileDataByIdAsync(id);

            if (file.Success)
            {
                return File(file.Stream, file.MimeType);
            }

            return Problem(file.Message);
        }


        [HttpPost]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostFile([FromForm] FileInputResource fileInputResource)
        {
            var file = await _fileService.SaveAsync(fileInputResource.File, fileInputResource.Name,
                fileInputResource.StorageType);

            FileOutputResource fileOutputResource = _mapper.Map<FileOutputResource>(file);
            return fileOutputResource.Success
                ? Created(fileOutputResource.Path, fileOutputResource)
                : Problem(fileOutputResource.Message);
        }

        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id)
        {
            FileOutputResource fileOutputResource = await _fileService.DeleteAsync(id);
            return fileOutputResource.Success ? Ok(fileOutputResource) : Problem(fileOutputResource.Message);
        }

        [HttpDelete]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteAllFile()
        {
            FileOutputResource fileOutputResource = await _fileService.DeleteAllAsync();
            return fileOutputResource.Success ? Ok(fileOutputResource) : Problem(fileOutputResource.Message);
        }

        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateFile([FromForm] FileInputResource fileInputResource, [FromRoute] int id)
        {
            FileOutputResource fileOutputResource = await _fileService.UpdateAsync(fileInputResource.File, id);
            return fileOutputResource.Success ? Ok(fileOutputResource) : Problem(fileOutputResource.Message);
        }
    }
}