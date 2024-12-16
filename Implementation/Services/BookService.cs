using BestStoreMVC.Data;
using BestStoreMVC.Dto;
using BestStoreMVC.Entity;
using BestStoreMVC.Implementation.Interface;
using BestStoreMVC.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMVC.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;

        public BookService(ApplicationDbContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(string? search, string? column, string? orderBy, int pageIndex, int pageSize)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Category);

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            // Sorting
            column ??= "Id";
            orderBy ??= "desc";

            query = column switch
            {
                "Name" => orderBy == "asc" ? query.OrderBy(b => b.Title) : query.OrderByDescending(b => b.Title),
                "Author" => orderBy == "asc" ? query.OrderBy(b => b.Author) : query.OrderByDescending(b => b.Author),
                "Category" => orderBy == "asc" ? query.OrderBy(b => b.Category.Name) : query.OrderByDescending(b => b.Category.Name),
                "Price" => orderBy == "asc" ? query.OrderBy(b => b.Price) : query.OrderByDescending(b => b.Price),
                "CreatedAt" => orderBy == "asc" ? query.OrderBy(b => b.CreatedAt) : query.OrderByDescending(b => b.CreatedAt),
                _ => orderBy == "asc" ? query.OrderBy(b => b.Id) : query.OrderByDescending(b => b.Id),
            };

            // Pagination
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await query.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                CategoryName = b.Category.Name,
                Price = b.Price,
                Language = b.Language,
                Pages = b.Pages,
                ISBN = b.ISBN,
                Description = b.Description,
                ImageFileName = b.ImageFileName // Images not returned for list
            }).ToListAsync();
        }

        public async Task<int> GetBookCountAsync(string? search)
        {
            IQueryable<Book> query = _context.Books;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            return await query.CountAsync();
        }

        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return null;

            return new BookDto
            {
                Title = book.Title,
                Author = book.Author,
                CategoryName = book.Category.Name,
                Price = book.Price,
                Language = book.Language,
                Pages = book.Pages,
                ISBN = book.ISBN,
                ImageFileName = book.ImageFileName,
                Description = book.Description
            };
        }

        public async Task<bool> CreateBookAsync(BookRequestModel request, string imagePath)
        {
            // Save image
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(request.ImageFile!.FileName);
            string imageFullPath = Path.Combine(imagePath, newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                request.ImageFile.CopyTo(stream);
            }

            // Save book to DB
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Language = request.Language,
                Pages = request.Pages,
                ISBN = request.ISBN,
                Description = request.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<BookDto> UpdateBookAsync(Guid id, BookRequestModel request, string imagePath)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            if (request.ImageFile != null)
            {
                // Save new image
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(request.ImageFile.FileName);
                string imageFullPath = Path.Combine(imagePath, newFileName);

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    request.ImageFile.CopyTo(stream);
                }

                // Delete old image
                string oldImageFullPath = Path.Combine(imagePath, book.ImageFileName);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }

                book.ImageFileName = newFileName;
            }

            // Update book
            book.Title = request.Title;
            book.Author = request.Author;
            book.CategoryId = request.CategoryId;
            book.Price = request.Price;
            book.Language = request.Language;
            book.Pages = request.Pages;
            book.ISBN = request.ISBN;
            book.Description = request.Description;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                Price = book.Price,
                Language = book.Language,
                Pages = book.Pages,
                ISBN = book.ISBN,
                Description = book.Description,
            };
        }

        public async Task DeleteBookAsync(Guid id, string imagePath)
        {

            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                // Delete image
                string imageFullPath = Path.Combine(imagePath, book.ImageFileName);
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

        }
    }
}
