using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Services
{
    public class ShoeService : IShoeService
    {
        private readonly ShoeStoreContext _context;

        public ShoeService(ShoeStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shoe>> GetAllShoesAsync()
        {
            return await _context.Shoes
                .Where(s => s.IsActive)
                .OrderBy(s => s.Brand)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Shoe?> GetShoeByIdAsync(int id)
        {
            return await _context.Shoes
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<Shoe> CreateShoeAsync(Shoe shoe)
        {
            shoe.CreatedAt = DateTime.UtcNow;
            shoe.UpdatedAt = DateTime.UtcNow;
            shoe.IsActive = true;
            
            _context.Shoes.Add(shoe);
            await _context.SaveChangesAsync();
            return shoe;
        }

        public async Task<bool> UpdateShoeAsync(int id, Shoe shoe)
        {
            var existingShoe = await _context.Shoes.FindAsync(id);
            if (existingShoe == null || !existingShoe.IsActive)
                return false;

            existingShoe.Name = shoe.Name;
            existingShoe.Brand = shoe.Brand;
            existingShoe.Price = shoe.Price;
            existingShoe.Size = shoe.Size;
            existingShoe.Color = shoe.Color;
            existingShoe.Category = shoe.Category;
            existingShoe.StockQuantity = shoe.StockQuantity;
            existingShoe.Description = shoe.Description;
            existingShoe.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteShoeAsync(int id)
        {
            var shoe = await _context.Shoes.FindAsync(id);
            if (shoe == null)
                return false;

            // М'яке видалення
            shoe.IsActive = false;
            shoe.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Shoe>> SearchShoesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllShoesAsync();

            searchTerm = searchTerm.ToLower();
            
            return await _context.Shoes
                .Where(s => s.IsActive && 
                    (s.Name.ToLower().Contains(searchTerm) ||
                     s.Brand.ToLower().Contains(searchTerm) ||
                     s.Color.ToLower().Contains(searchTerm) ||
                     s.Category.ToLower().Contains(searchTerm) ||
                     (s.Description != null && s.Description.ToLower().Contains(searchTerm))))
                .OrderBy(s => s.Brand)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shoe>> GetShoesByBrandAsync(string brand)
        {
            return await _context.Shoes
                .Where(s => s.IsActive && s.Brand.ToLower() == brand.ToLower())
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shoe>> GetShoesByCategoryAsync(string category)
        {
            return await _context.Shoes
                .Where(s => s.IsActive && s.Category.ToLower() == category.ToLower())
                .OrderBy(s => s.Brand)
                .ThenBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<bool> UpdateStockAsync(int id, int quantity)
        {
            var shoe = await _context.Shoes.FindAsync(id);
            if (shoe == null || !shoe.IsActive)
                return false;

            shoe.StockQuantity = quantity;
            shoe.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

