using Cascade.Assessment.Api.DataAccess.Entities;
using Cascade.Assessment.Api.Interfaces;
using Cascade.Assessment.Api.Model;
using Cascade.Assessment.Api.Extensions;
using Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cascade.Assessment.Api.DataAccess
{
    public class BookRepository : IBookRepository
    {
        private readonly ILogger<BookRepository> _logger;
        private readonly IConfiguration _config;

        public BookRepository(ILogger<BookRepository> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /**
         * CreateBookAuthorAndPublisher method creates DB entitiy objects of Books, Authors and Publishers to be able to insert into the database
         * The return value contains Tuple of list of Book, Author and publisher
         * */
        (IEnumerable<BookEntity>, IEnumerable<AuthorEntity>, IEnumerable<PublisherEntity>) CreateBookAuthorAndPublisher(IEnumerable<IBook> books)
        {
            _logger.LogInformation($"In CreateBookAuthorAndPublisher. List Count:{books.Count()}");
            try
            {
                var publishers = books.GroupBy(b => b.Publisher.ToCamelCase());

                var newPublishers = new List<PublisherEntity>();
                var newAuthors = new List<AuthorEntity>();
                var newBooks = new List<BookEntity>();

                foreach (var item in publishers)
                {
                    var guid = Guid.NewGuid();
                    var p = new PublisherEntity { PublisherId = guid, Publisher = item.Key };
                    newPublishers.Add(p);
                    foreach (var b in item)
                    {
                        var author = newAuthors.Where(a => a.AuthorFirstName.ToCamelCase() == b.AuthorFirstName.ToCamelCase() && a.AuthorLastName.ToCamelCase() == b.AuthorLastName.ToCamelCase()).FirstOrDefault();
                        if (author == null)
                        {
                            author = new AuthorEntity { AuthorId = Guid.NewGuid(), AuthorLastName = b.AuthorLastName.ToCamelCase(), AuthorFirstName = b.AuthorFirstName.ToCamelCase() };
                            newAuthors.Add(author);
                        }

                        var newBook = new BookEntity
                        {
                            Title = b.Title,
                            PublisherId = p.PublisherId,
                            Price = b.Price,
                            AuthorId = author.AuthorId,
                            BookEdition = b.BookEdition,
                            PublishedLocation = b.PublishedLocation,
                            SubTitle = b.SubTitle,
                            MaximumPages = b.MaximumPages,
                            MinimumPages = b.MinimumPages,
                            PublishedDate = b.PublishedDate
                        };
                        newBooks.Add(newBook);
                    }
                }
                return (newBooks, newAuthors, newPublishers);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In CreateBookAuthorAndPublisher. List Count:{books.Count()}");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out CreateBookAuthorAndPublisher. List Count:{books.Count()}");
            }
        }

        string ConnectionString => _config.GetConnectionString("BookConnectionString");
        public async Task<int> AddBooksAsync(IEnumerable<IBook> list)
        {
            _logger.LogInformation($"In AddBooksAsync. List Count:{list.Count()}");
            try
            {
                var (books, authors, publishers) = CreateBookAuthorAndPublisher(list);

                _logger.LogInformation($"In AddBooksAsync. Book Count:{books.Count()}, Authors Count:{authors.Count()}, Publishers Count:{publishers.Count()}");

                //Convert the collectio to JSON
                var booksJson = JsonConvert.SerializeObject(books);
                var authorsJson = JsonConvert.SerializeObject(authors);
                var publishersJson = JsonConvert.SerializeObject(publishers);

                using var sqlConnection = new SqlConnection(ConnectionString);

                //Send the json to the stored proc to insert the data
                var parameters = new DynamicParameters();
                var result = -1;
                parameters.Add("jsonBooks", booksJson);
                parameters.Add("jsonAuthors", authorsJson);
                parameters.Add("jsonPublishers", publishersJson);


                parameters.Add("result", direction: System.Data.ParameterDirection.Output, dbType: System.Data.DbType.Int32);
                var r = await sqlConnection.ExecuteAsync("dbo.usp_AddBooks", parameters, commandType: System.Data.CommandType.StoredProcedure);
                result = parameters.Get<int>("result");
                if (result == 0)
                {
                    throw new Exception("Adding Books Failed");
                }
                _logger.LogInformation($"In AddBooksAsync. Records Inserted:{r}");
                return r;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In AddBooksAsync.");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out AddBooksAsync.");
            }
        }

        public async Task<IEnumerable<IBook>> GetBooksAsync(string criteria, string sortBy)
        {
            _logger.LogInformation($"In GetBooksAsync. sort by:{sortBy}, selectCriteria:{criteria}");
            try
            {
                using var sqlConnection = new SqlConnection(ConnectionString);
                var parameters = new DynamicParameters();

                parameters.Add("selectCriteria", criteria);
                parameters.Add("sortCriteria", sortBy);

                return await sqlConnection.QueryAsync<Book>("dbo.usp_GetBooks", parameters, commandType: System.Data.CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In GetBooksAsync. sort by:{sortBy}, selectCriteria:{criteria}");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out GetBooksAsync. sort by:{sortBy}, selectCriteria:{criteria}");
            }
        }

        public async Task<decimal> GetTotalBooksPriceAsync()
        {
            _logger.LogInformation($"In GetTotalBooksPriceAsync.");
            try
            {
                using var sqlConnection = new SqlConnection(ConnectionString);
                return await sqlConnection.ExecuteScalarAsync<decimal>("dbo.usp_GetTotalBooksPrice", commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception In GetTotalBooksPriceAsync.");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out GetTotalBooksPriceAsync.");
            }
        }
    }
}
