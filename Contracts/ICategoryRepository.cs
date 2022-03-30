
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICategoryRepository
    {
        Task<PagedList<Category>> GetCategoriesAsync(CategoryQueryParameters categoryParameters);

        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<bool> CategoryExistAsync(Category category);

        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
