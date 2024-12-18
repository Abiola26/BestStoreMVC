﻿using BestStoreMVC.Data;
using BestStoreMVC.Dto;
using BestStoreMVC.Entity;
using BestStoreMVC.Implementation.Interface;
using BestStoreMVC.Models.RequestModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMVC.Implementation.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public async Task<CategoryDto> GetCategoryByNameAsync(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(a => a.Name == categoryName);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryRequestModel categoryRequest)
        {
            var category = new Category { Name = categoryRequest.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, CategoryRequestModel categoryRequest)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            category.Name = categoryRequest.Name;
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategorySelectList()
        {
            var categories = await GetAllCategoriesAsync();
            var categoryList = categories.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });

            return new SelectList(categoryList, "Value", "Text");
        }
    }

}
