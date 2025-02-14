using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO.BlogPostDtos;
using CodePulse.API.Models.DTO.CategoryDtos;
using CodePulse.API.Repository.Implementation;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = "Writer")]
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
                    FeaturedImageUrl = blog.FeaturedImageUrl,
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

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetById(id);

            if(blogPost is null)
            {
                return NoContent();
            }

            //domain to mdto
            var response = new BlogPostResponseDto()
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                Title = blogPost.Title,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
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
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandle(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }
            
            var response = new BlogPostResponseDto()
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                IsVisible = blogPost.IsVisible,
                UrlHandle = blogPost.UrlHandle,
                Title = blogPost.Title,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Category = blogPost.Categories.Select(x => new CategoryResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlog([FromRoute] Guid id, UpdateBlogPostRequestDto editPost)
        {
            var request = new BlogPost()
            {
                Id = id,
                Author = editPost.Author,
                Content = editPost.Content,
                ShortDescription = editPost.ShortDescription,
                PublishedDate = editPost.PublishedDate,
                IsVisible = editPost.IsVisible,
                UrlHandle = editPost.UrlHandle,
                Title = editPost.Title,
                FeaturedImageUrl = editPost.FeaturedImageUrl,
                Categories = new List<Category>()
            };

          
            foreach(var category in editPost.Category)
            {
                var categoryFound = await categoryRepository.GetByIdAsync(category);
                
                if(categoryFound is not null)
                {
                    request.Categories.Add(categoryFound);
                }
            }

            var blogpost = await blogPostRepository.UpdateBlogPost(request);

            if (blogpost is null)
            {
                return NotFound();
            }

            var response = new BlogPostResponseDto()
            {
                Id = blogpost.Id,
                Author = blogpost.Author,
                Content = blogpost.Content,
                ShortDescription = blogpost.ShortDescription,
                PublishedDate = blogpost.PublishedDate,
                IsVisible = blogpost.IsVisible,
                UrlHandle = blogpost.UrlHandle,
                Title = blogpost.Title,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                Category = blogpost.Categories.Select(x => new CategoryResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };


            return Ok(response);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid id)
        {
            var existingBlogPost = await blogPostRepository.DeleteAsync(id);

            if (existingBlogPost is null)
            {
                return NotFound();
            }

            return Ok();

        }

    }
}
