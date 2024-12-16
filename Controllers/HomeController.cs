using BestStoreMVC.Data;
using BestStoreMVC.Dto;
using BestStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BestStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var books = context.Books
        .Include(b => b.Category) // Include Category to get CategoryName
        .OrderByDescending(p => p.Id)
        .Take(4)
        .Select(b => new BookDto
        {
            Id = b.Id, // `Id` as GUID
            Title = b.Title,
            Author = b.Author,
            CategoryName = b.Category.Name, // Adjusted for Category navigation property
            Price = b.Price,
            ImageFileName = b.ImageFileName // Adjust to match your entity's property
        })
        .ToList();

            return View(books);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}