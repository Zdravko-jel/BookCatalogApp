using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly BookCatalogDBContext _context;

        public ReviewService(BookCatalogDBContext context)
        {
            _context = context;
        }

        public void AddReview(int bookId, int? userId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                throw new Exception("Rating must be between 1 and 5!");
            }

            var review = new Review { 
                BookId = bookId,
                UserId = userId,
                Rating = rating,
                Content = comment
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public double GetAvarageRating(int bookId)
        {
            var reviews = _context.Reviews.Where(r => r.BookId == bookId);

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }

        public List<Review> GetByBook(int bookId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .ToList();
        }
    }
}
