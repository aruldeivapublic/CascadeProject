using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cascade.Assessment.Api.DataAccess.Entities
{
    public class BookEntity
    {
        public long BookId { get; set; }
        public Guid PublisherId { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public decimal Price { get; set; }
        public int MinimumPages { get; set; }
        public int MaximumPages { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedLocation { get; set; }
        public string BookEdition { get; set; }

    }
}
