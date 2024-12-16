using BestStoreMVC.Entity;
using BestStoreMVC.Models.RequestModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BestStoreMVC.Implementation.Interface;


namespace BestStoreMVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;
        private readonly INotyfService _notyf;
        private readonly int pageSize = 5;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment environment, INotyfService notyf)
        {
            _categoryService = categoryService;
            _environment = environment;
            _notyf = notyf;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            //IQueryable<Category> query = _categoryService.GetAllCategoriesAsync;

            //// search functionality
            //if (search != null)
            //{
            //    query = query.Where(p => p.Title.Contains(search) || p.Author.Contains(search));
            //}

            //// sort functionality
            //string[] validColumns = { "Id", "Name", "Author", "Category", "Price", "CreatedAt" };
            //string[] validOrderBy = { "desc", "asc" };

            //if (!validColumns.Contains(column))
            //{
            //    column = "Id";
            //}

            //if (!validOrderBy.Contains(orderBy))
            //{
            //    orderBy = "desc";
            //}

            //if (column == "Name")
            //{
            //    if (orderBy == "asc")
            //    {
            //        query = query.OrderBy(p => p.Title);
            //    }
            //    else
            //    {
            //        query = query.OrderByDescending(p => p.Title);
            //    }
            //}
            //else if (column == "Author")
            //{
            //    if (orderBy == "asc")
            //    {
            //        query = query.OrderBy(p => p.Author);
            //    }
            //    else
            //    {
            //        query = query.OrderByDescending(p => p.Author);
            //    }
            //}
            ////else if (column == "Category")
            ////{
            ////    if (orderBy == "asc")
            ////    {
            ////        query = query.OrderBy(p => p.Category);
            ////    }
            ////    else
            ////    {
            ////        query = query.OrderByDescending(p => p.Category);
            ////    }
            ////}
            //else if (column == "Price")
            //{
            //    if (orderBy == "asc")
            //    {
            //        query = query.OrderBy(p => p.Price);
            //    }
            //    else
            //    {
            //        query = query.OrderByDescending(p => p.Price);
            //    }
            //}
            //else if (column == "CreatedAt")
            //{
            //    if (orderBy == "asc")
            //    {
            //        query = query.OrderBy(p => p.CreatedAt);
            //    }
            //    else
            //    {
            //        query = query.OrderByDescending(p => p.CreatedAt);
            //    }
            //}
            //else
            //{
            //    if (orderBy == "asc")
            //    {
            //        query = query.OrderBy(p => p.Id);
            //    }
            //    else
            //    {
            //        query = query.OrderByDescending(p => p.Id);
            //    }
            //}

            ////query = query.OrderByDescending(p => p.Id);

            ////pagination functionality
            //if (pageIndex < 1)
            //{
            //    pageIndex = 1;
            //}

            //decimal count = query.Count();
            //Guid totalPages = (Guid)Math.Ceiling(count / pageSize);
            //query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            //var Books = query.ToList();

            //ViewData["PageIndex"] = pageIndex;
            //ViewData["TotalPages"] = totalPages;

            //ViewData["Search"] = search ?? "";

            //ViewData["Column"] = column;
            //ViewData["OrderBy"] = orderBy;
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequestModel categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryService.CreateCategoryAsync(categoryRequest);
                _notyf.Success("Category Created Succesfully");
                return RedirectToAction(nameof(Index));
            }
            return View(categoryRequest);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryRequest = new CategoryRequestModel
            {
                Name = category.Name
            };
            return View(categoryRequest);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CategoryRequestModel categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
                _notyf.Success("Category Updated Succesfully");
                if (updatedCategory == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoryRequest);
        }

        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            _notyf.Success("Category has been successfully deleted.");
            return RedirectToAction(nameof(Index));
        }
    }
}
