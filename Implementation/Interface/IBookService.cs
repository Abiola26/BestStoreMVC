using BestStoreMVC.Dto;
using BestStoreMVC.Models.RequestModels;

namespace BestStoreMVC.Implementation.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(string? search, string? column, string? orderBy, int pageIndex, int pageSize);
        Task<int> GetBookCountAsync(string? search);
        Task<BookDto?> GetBookByIdAsync(Guid id);
        Task<bool> CreateBookAsync(BookRequestModel request, string imagePath);
        Task<BookDto> UpdateBookAsync(Guid id, BookRequestModel request, string imagePath);
        Task DeleteBookAsync(Guid id, string imagePath);
    }
}
