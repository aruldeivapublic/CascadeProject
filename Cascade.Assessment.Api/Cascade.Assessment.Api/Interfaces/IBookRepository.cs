using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cascade.Assessment.Api.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<IBook>> GetBooksAsync(string criteria, string sortBy);

        Task<int> AddBooksAsync(IEnumerable<IBook> list);

        Task<decimal> GetTotalBooksPriceAsync();
    }
}
