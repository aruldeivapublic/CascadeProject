using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cascade.Assessment.Api.Interfaces
{
    public interface IBook
    {
        string Publisher { get; set; }
        string Title { get; set; }
        public string SubTitle { get; set; }
        string AuthorLastName { get; set; }
        string AuthorFirstName { get; set; }
        decimal Price { get; set; }
        public int MinimumPages { get; set; }
        public int MaximumPages { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedLocation { get; set; }
        public string BookEdition { get; set; }

        string MlaStyleCitation { get;}
        string ChicagoStyleCitation { get;}
    }
}
