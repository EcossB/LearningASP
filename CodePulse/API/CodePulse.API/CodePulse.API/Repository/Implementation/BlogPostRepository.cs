using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {

        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);

            await dbContext.SaveChangesAsync();

            return blogPost;
        }


        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetById(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(bId => bId.Id == id);
        }

        public async Task<BlogPost?> UpdateBlogPost(BlogPost blogPost)
        {
            var existinBlog = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existinBlog is not null)
            {
                //updating the blogpost
                dbContext.Entry(existinBlog).CurrentValues.SetValues(blogPost);

                // also updating the categories of the currently blogpost 
                existinBlog.Categories = blogPost.Categories;
                await dbContext.SaveChangesAsync();
                return blogPost;
            }

            return null; 
        }
    

    }
}
