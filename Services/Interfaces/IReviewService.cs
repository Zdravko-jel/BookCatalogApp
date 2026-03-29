using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReviewService
    {
        void AddReview(int bookId, int? userId, int rating, string comment);

        List<Review> GetByBook(int bookId);

        double GetAvarageRating(int bookId);
    }
}
