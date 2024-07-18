using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface IBlogPostRepository
    {
        public Task<BlogPost> CreateAsync(BlogPost blogPost);

        public Task<IEnumerable<BlogPost>> GetAllAsync();
    }
}
