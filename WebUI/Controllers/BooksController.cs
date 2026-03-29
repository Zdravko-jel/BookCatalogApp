using Data;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public BooksController(IBookService bookService, IReviewService reviewService, IUserService userService )
        {
            _bookService = bookService;
            _reviewService = reviewService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAll();

            return View(books);
        }

        public IActionResult Details(int id) { 
            var book = _bookService.GetById(id);
            var avgRating = _reviewService.GetAvarageRating(id);

            ViewBag.AvarageRating = $"{avgRating:f2}";

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            _bookService.Add(book);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddReview(int bookId, int rating, string comment, string? username, string? email)
        {
            int userId = -1;

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
                userId = -1;
            else
            {
                var user = _userService.GetByEmail(email);
                if (user is null)
                {
                    _userService.AddUser(username, email);
                    user = _userService.GetByEmail(email);
                    userId = user.Id;
                }
                else
                {
                    userId = user.Id;
                }
            }

            _reviewService.AddReview(bookId, (userId == -1 ? null : userId), rating, comment);
            return RedirectToAction("Details", new { id = bookId });
        }

        public IActionResult Edit(int id)
        {
            var book = _bookService.GetById(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            _bookService.Update(book);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var book = _bookService.GetById(id);
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var books = _bookService.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                books = books
                    .Where(b => b.Title.Contains(search))
                    .ToList();
            }

            return View(books);
        }
    }
}
