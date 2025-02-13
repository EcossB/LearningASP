using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPostDtos;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : Controller
{
    private readonly IImageRepository _imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetImages()
    {
        var imageList = await _imageRepository.GetAll();
        var response = new List<BlogImageDto>();

        foreach (var image in imageList)
        {
            response.Add(new BlogImageDto
            {
                Id = image.Id,
                FileName = image.FileName,
                FileExtension = image.FileExtension,
                Title = image.Title,
                Url = image.Url,
                DateCreated = image.DateCreated
            });
        }
        
        return Ok(response);

    }
    
    
    // post
    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
    {
        ValidateFileUpload(file);

        if (ModelState.IsValid)
        {
            var blogImage = new BlogImage()
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };
        
            blogImage = await _imageRepository.Upload(file,blogImage);
            //converting domainModel to DTO
            var blogImageResponse = new BlogImageDto
            {
                Id = blogImage.Id,
                FileExtension = blogImage.FileExtension,
                FileName = blogImage.FileName,
                Title = blogImage.Title,
                DateCreated = blogImage.DateCreated,
                Url = blogImage.Url
            };
            
            return Ok(blogImageResponse);
        }
        
        return BadRequest(ModelState);
        
    }

    private void ValidateFileUpload(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            ModelState.AddModelError("file", "unsupported file extension");
        }

        if (file.Length > 10485760)
        {
            ModelState.AddModelError("file", "file size cannot be more than 10Mb");
        }
    }
}