using Cascade.Assessment.Api.Interfaces;
using Cascade.Assessment.Api.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cascade.Assessment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IConfiguration _config;
        private readonly IBookRepository _repository;
        public BooksController(ILogger<BooksController> logger, IConfiguration config, IBookRepository repository)
        {
            _logger = logger;
            _config = config;
            _repository = repository;

        }
        // GET: api/<BooksController>
        [HttpGet]
        [Route("ListByPublisherAuthorTitle")]
        public async Task<ActionResult<IEnumerable<IBook>>> ListByPublisherAuthorTitle()
        {
            _logger.LogInformation($"In ListByPublisherAuthorTitle");
            try
            {
                var result = await _repository.GetBooksAsync("", "publisher,authorlastname,authorfirstname,title");
                _logger.LogInformation($"In ListByPublisherAuthorTitle. Result Count:{result.Count()}");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In ListByPublisherAuthorTitle");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out ListByPublisherAuthorTitle");
            }
        }

        [HttpGet]
        [Route("ListByAuthorTitle")]
        public async Task<ActionResult<IEnumerable<IBook>>> ListByAuthorTitle()
        {
            _logger.LogInformation($"In ListByAuthorTitle");
            try
            {
                var result = await _repository.GetBooksAsync("", "authorlastname,authorfirstname,title");
                _logger.LogInformation($"In ListByAuthorTitle. Result Count:{result.Count()}");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In ListByAuthorTitle");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out ListByAuthorTitle");
            }
        }

        [HttpGet]
        [Route("TotalPrice")]
        public async Task<ActionResult<decimal>> TotalPrice()
        {
            _logger.LogInformation($"In TotalPrice");
            try
            {
                var result = await _repository.GetTotalBooksPriceAsync();
                _logger.LogInformation($"In TotalPrice. Result:{result}");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In TotalPrice");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out TotalPrice");
            }
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] IEnumerable<Book> value)
        {
            _logger.LogInformation($"In Post to Add Books. Input count:{value.Count()}");
            try
            {
                var result = await _repository.AddBooksAsync(value);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception In Post to Add Books");
                throw;
            }
            finally
            {
                _logger.LogInformation($"Out Post to Add Books");
            }
        }

    }
}
