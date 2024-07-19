using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPostDtos;
using CodePulse.API.Models.DTO.CategoryDtos;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : Controller
    {
        
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateBlog(AddBlogPostRequest request)
        {
            //mapping dto to model
            var newBlog = new BlogPost()
            {
               Title = request.Title,
               ShortDescription = request.ShortDescription,
               Content = request.Content,
               FeaturedImageUrl = request.FeaturedImageUrl,
               UrlHandle = request.UrlHandle,
               PublishedDate = request.PublishedDate,
               Author = request.Author,
               IsVisible = request.IsVisible,
               Categories = new List<Category>()
            };

            foreach(var category in request.Category)
            {
                var existingCategory = await categoryRepository.GetByIdAsync(category);
                if (existingCategory is not null)
                {
                    newBlog.Categories.Add(existingCategory);
                }

            }


            var blogPost = await blogPostRepository.CreateAsync(newBlog);

            var response = new AddBlogPostResponseDto()
            {
                Id = blogPost.Id,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Category = blogPost.Categories.Select(x => new CategoryResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var listBlogs = await blogPostRepository.GetAllAsync();
            var response = new List<AddBlogPostResponseDto>();

            foreach(var blog in listBlogs)
            {

                response.Add(new AddBlogPostResponseDto
                {
                    Id = blog.Id,
                    Author = blog.Author,
                    Content = blog.Content,
                    ShortDescription = blog.ShortDescription,
                    PublishedDate = blog.PublishedDate,
                    IsVisible = blog.IsVisible,
                    UrlHandle = blog.UrlHandle,
                    Title = blog.Title,
                    Category = blog.Categories.Select(x => new CategoryResponseDto() 
                    { 
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });

            }

            return Ok(response);
        }

    }
}
