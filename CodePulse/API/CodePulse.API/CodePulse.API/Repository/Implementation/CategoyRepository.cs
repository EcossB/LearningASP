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

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            //Category newCategory = await dbContext.Categories.Where(c => c.Id == category.Id).FirstAsync();

            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if(existingCategory is not null)
            {
                /*existingCategory.Id = category.Id;
                existingCategory.Name = category.Name;
                existingCategory.UrlHandle = category.UrlHandle; a way to update a instance. */ 

                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }

            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(existingCategory is not null)
            {
                dbContext.Categories.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            } 

            return null;
            
        }
    }
}
