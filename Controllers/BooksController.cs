using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using BestStoreMVC.Implementation.Interface;
using BestStoreMVC.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BestStoreMVC.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _environment;
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notyf;
        private const int PageSize = 5;

        public BooksController(IBookService bookService, IWebHostEnvironment environment, ICategoryService categoryService, INotyfService notyf)
        {
            _bookService = bookService;
            _environment = environment;
            _categoryService = categoryService;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, string? search = null, string? column = null, string? orderBy = null)
        {
            var books = await _bookService.GetAllBooksAsync(search, column, orderBy, pageIndex, PageSize);
            var totalCount = await _bookService.GetBookCountAsync(search);

            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / PageSize);
            ViewData["Search"] = search ?? "";
            ViewData["Column"] = column ?? "Id";
            ViewData["OrderBy"] = orderBy ?? "desc";

            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(request);
            }

            var imagePath = Path.Combine(_environment.WebRootPath, "Books");
            await _bookService.CreateBookAsync(request, imagePath);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", book.CategoryId);

            var requestModel = new BookRequestModel
            {
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                Price = book.Price,
                Language = book.Language,
                Pages = book.Pages,
                ISBN = book.ISBN,
                Description = book.Description
            };

            return View(requestModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BookRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", request.CategoryId);
                return View(request);
            }

            var imagePath = Path.Combine(_environment.WebRootPath, "Books");
            var success = await _bookService.UpdateBookAsync(id, request, imagePath);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(_environment.WebRootPath, "Books");
            await _bookService.DeleteBookAsync(id, imagePath);

            _notyf.Success("Book deleted successfully.");

            return RedirectToAction(nameof(Index));
        }
    }
}
