using Cascade.Assessment.Api.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cascade.Assessment.Api.Model
{
    public class Book : IBook
    {
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorFirstName { get; set; }
        public decimal Price { get; set; }

        public int MinimumPages { get; set; }
        public int MaximumPages { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedLocation { get; set; }
        public string BookEdition { get; set; }
        public string MlaStyleCitation
        {
            get
            {
                var citation = $"{AuthorLastName}, {AuthorFirstName}. \"{Title}\".{Environment.NewLine}";
                citation += (BookEdition?.Length) > 0 ? $"{BookEdition}, " : "";
                citation += $"{Publisher}, {PublishedDate.Year}, pp {MinimumPages}-{MaximumPages}.";
                return citation;
            }
        }
        public string ChicagoStyleCitation
        {
            get
            {
                var citation = $"{AuthorLastName}, {AuthorFirstName}. {Title}.";
                citation += (SubTitle?.Length) > 0 ? $":{SubTitle}." : "";
                citation += (BookEdition?.Length) > 0 ? $":{BookEdition}." : "";
                citation += $"{PublishedLocation}:{Publisher}, {PublishedDate.Year}.";
                return citation;
            }
        }
    }
}
