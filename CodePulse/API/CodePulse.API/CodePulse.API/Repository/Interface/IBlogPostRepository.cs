using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface IBlogPostRepository
    {
        public Task<BlogPost> CreateAsync(BlogPost blogPost);

        public Task<IEnumerable<BlogPost>> GetAllAsync();

        public Task<BlogPost?> GetById(Guid id);

        public Task<BlogPost?> UpdateBlogPost(BlogPost blogPost);

        public Task<BlogPost?> DeleteAsync(Guid id);
    }
}
