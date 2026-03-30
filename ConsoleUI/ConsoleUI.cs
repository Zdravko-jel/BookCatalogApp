using Data;
using Services.Implementations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class ConsoleUI
    {
        private IBookService bookService;
        private IUserService userService;
        private IReviewService reviewService;

        public ConsoleUI(BookCatalogDBContext context)
        {
            bookService = new BookService(context);
            userService = new UserService(context);
            reviewService = new ReviewService(context);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n=== BOOK CATALOG ===");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. List Books");
                Console.WriteLine("2. Add Book");
                Console.WriteLine("3. View Book Details");
                Console.WriteLine("4. Add Review");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("0. Exit");
                Console.ResetColor();

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListBooks(); break;
                    case "2": AddBook(); break;
                    case "3": ViewDetails(); break;
                    case "4": AddReview(); break;
                    case "0": return;
                }
            }
        }

        public void ListBooks()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("List of the books in the system:");
            Console.WriteLine();

            var books = bookService.GetAll();

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Id}. {book.Title} - {book.Authors}");
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }

        public void AddBook()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Add a book: ");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Title: ");
            Console.ResetColor();
            var title = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Authors: ");
            Console.ResetColor();
            var authors = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Genres: ");
            Console.ResetColor();
            var genres = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Year: ");
            Console.ResetColor();
            var year = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Image URL (optional): ");
            Console.ResetColor();
            var image = Console.ReadLine();

            var book = new Book
            {
                Title = title,
                Authors = authors,
                Genres = genres,
                Year = year,
                ImageUrl = image
            };

            bookService.Add(book);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Book added!");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }

        public void ViewDetails()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Details: ");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter book ID: ");
            int id = int.Parse(Console.ReadLine());

            var book = bookService.GetById(id);

            if (book == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book not found.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n{book.Title}");
            Console.WriteLine($"Author: {book.Authors}");
            Console.WriteLine($"Genre: {book.Genres}");
            Console.WriteLine($"Year: {book.Year}");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            var avg = reviewService.GetAvarageRating(id);
            Console.WriteLine($"⭐ Average Rating: {avg:F1}");

            Console.WriteLine();

            Console.WriteLine("\nReviews:");

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var r in book.Reviews)
            {
                var user = r.User != null ? r.User.Username : "Anonymous";
                Console.WriteLine($"- {user}: {r.Rating}/5");
                Console.WriteLine($"  {r.Content}");
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }

        void AddReview()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Add a review to a book:");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Book ID: ");
            Console.ResetColor();
            int bookId = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Username (leave empty for anonymous): ");
            Console.ResetColor();
            var userInput = Console.ReadLine();

            string? username = string.IsNullOrEmpty(userInput) ? null : userInput;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Email (leave empty for anonymous): ");
            Console.ResetColor();
            var userEmail = Console.ReadLine();

            string? email = string.IsNullOrEmpty(userEmail) ? null : userEmail;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Rating (1-5): ");
            Console.ResetColor();
            int rating = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Comment: ");
            Console.ResetColor();
            string comment = Console.ReadLine();

            int userId = -1;

            if (!(username is null))
            {
                var user = userService.GetByEmail(email);
                if (user is null)
                {
                    userService.AddUser(username, email);
                    user = userService.GetByEmail(email);
                    userId = user.Id;
                }
                else
                {
                    userId = user.Id;   
                }
            }
            else
            {
                userId = -1;
            }

            reviewService.AddReview(bookId, userId, rating, comment);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Review added!");

            Console.WriteLine();

            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }
    }
}
