using LinkHub.Api.Dtos;
using LinkHub.Api.Infrastructure.Data;
using LinkHub.Api.Services.Interfaces;
using LinkHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkHub.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(CreateCategoryDto categoryDto, int userId)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Color = categoryDto.Color,
                UserId = userId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> GetByIdAsync(int id, int userId)
        {
            return await _context.Categories
                                .AsNoTracking() 
                                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }
    }
}
