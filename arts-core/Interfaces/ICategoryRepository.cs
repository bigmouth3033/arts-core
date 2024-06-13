using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<CustomResult> GetAllCategories();

        CustomResult CreateNewCategory(Category category);
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(DataContext dataContext, ILogger<CategoryRepository> logger) : base(dataContext)
        {
            _logger = logger;
        }

        public CustomResult CreateNewCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);

                return new CustomResult(200, "success", category);

            }catch(Exception ex)
            {
                return new CustomResult(400, "failed", ex.Message);
            }
        }

        public async Task<CustomResult> GetAllCategories()
        {
            try
            {
                var list = await _context.Categories.ToListAsync();

                return new CustomResult(200, "success", list);
                
            }catch(Exception ex)
            {
                return new CustomResult(400, "bad request", ex.Message);
            }
        }
    }
}
