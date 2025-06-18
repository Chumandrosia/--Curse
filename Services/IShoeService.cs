using ShoeStore.Models;

namespace ShoeStore.Services
{
    public interface IShoeService
    {
        Task<IEnumerable<Shoe>> GetAllShoesAsync();
        Task<Shoe?> GetShoeByIdAsync(int id);
        Task<Shoe> CreateShoeAsync(Shoe shoe);
        Task<bool> UpdateShoeAsync(int id, Shoe shoe);
        Task<bool> DeleteShoeAsync(int id);
        Task<IEnumerable<Shoe>> SearchShoesAsync(string searchTerm);
        Task<IEnumerable<Shoe>> GetShoesByBrandAsync(string brand);
        Task<IEnumerable<Shoe>> GetShoesByCategoryAsync(string category);
        Task<bool> UpdateStockAsync(int id, int quantity);
    }
}