using BestStoreMVC.Data;
using BestStoreMVC.Entity;
using BestStoreMVC.Implementation.Interface;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BestStoreMVC.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IBookService _bookService;
        private readonly int pageSize = 8;

        public StoreController(ApplicationDbContext context, IBookService bookService)
        {
            this.context = context;
            _bookService = bookService;
        }

        public IActionResult Index(int pageIndex, string? search, string? Author, string? category, string? sort)
        {
            IQueryable<Book> query = context.Books;

            // search functionality
            if (search != null && search.Length > 0)
            {
                query = query.Where(p => p.Title.Contains(search));
            }


            // filter functionality
            if (Author != null && Author.Length > 0)
            {
                query = query.Where(p => p.Author.Contains(Author));
            }

            if (category != null && category.Length > 0)
            {
                query = query.Where(p => p.Category.Name.Contains(category));
            }

            // sort functionality
            if (sort == "price_asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (sort == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                // newest Books first
                query = query.OrderByDescending(p => p.Id);
            }

            

            // pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);


            var Books = query.ToList();

            ViewBag.Books = Books;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            var storeSearchModel = new StoreSearchModel()
            {
                Search = search,
                Author = Author,
                Category = category,
                Sort = sort
            };

            return View(storeSearchModel);
        }


        //public IActionResult Details(GUID id)
        //{
        //    var Book = context.Books.Find(id);
        //    if (Book == null)
        //    {
        //        return RedirectToAction("Index", "Store");
        //    }

        //    return View(Book);
        //}

        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return RedirectToAction("Index", "Store");
            }
            return View(book);
        }
    }
}
