using System;

namespace Cascade.Assessment.Api.DataAccess.Entities
{
    public class AuthorEntity
    {
        public Guid AuthorId { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorFirstName { get; set; }
    }
}
