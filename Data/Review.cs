using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } //1-5

        public string Content { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
