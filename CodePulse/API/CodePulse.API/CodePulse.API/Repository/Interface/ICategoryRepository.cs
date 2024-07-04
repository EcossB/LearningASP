using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface ICategoryRepository
    {
        /*This interface its going to be implemented on the categoryRepository class.*/
        Task<Category> CreateAsync(Category category);
    }
}
