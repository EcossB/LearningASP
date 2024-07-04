using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class CategoyRepository : ICategoryRepository
    {
        /*
         in the repository we work directlty we the domain model. 
        Because this layer works as an access layer to the data 
        so its better and a good practice to use the domains directly here 
        cause we are working directly with the data.
         */
        private readonly ApplicationDbContext dbContext;

        public CategoyRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;     
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
    }
}
