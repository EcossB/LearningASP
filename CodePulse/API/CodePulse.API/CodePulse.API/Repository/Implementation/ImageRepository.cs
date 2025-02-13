using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation;

public class ImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _dbcontext;

    public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbcontext)
    {
        _webHostEnvironment = webHostEnvironment; // this is to acces the project file enviroment and get a path
        _httpContextAccessor = httpContextAccessor; // this is to know if we are using http or https and also we can get the host name and base (.com). 
        _dbcontext = dbcontext; // de context to manipulate the database.
    }
    
    public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
    {
        // 1- upload the image to api/image
        var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "images", $"{blogImage.FileName}{blogImage.FileExtension}"); // here we are creating the path.
        await using var stream = new FileStream(localPath, FileMode.Create); // here we create the location 
        await file.CopyToAsync(stream); // here we copy the image in the stream we created. 
        
        
        // 2- update the database 
        // we are going to retreive the image in this way //https://codepulse.com/images/imagefile.jpg
        
        var requesteHttp = _httpContextAccessor.HttpContext.Request;
        var urlPath = $"{requesteHttp.Scheme}://{requesteHttp.Host}{requesteHttp.PathBase}/images/{blogImage.FileName}{blogImage.FileExtension}";
        
        blogImage.Url = urlPath;
        await _dbcontext.BlogImages.AddAsync(blogImage);
        await _dbcontext.SaveChangesAsync();
        return blogImage;
        
    }

    public async Task<List<BlogImage>> GetAll()
    {
        return await _dbcontext.BlogImages.ToListAsync();
    }
    
}