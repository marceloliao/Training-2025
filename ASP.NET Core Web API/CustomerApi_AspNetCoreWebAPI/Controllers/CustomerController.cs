using CustomerApi_AspNetCoreWebAPI.Data;
using CustomerApi_AspNetCoreWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerApi_AspNetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMongoCollection<Customer>? _customers;
        public CustomerController(MongoDbService mongoDbService)
        {
            _customers = mongoDbService.Database?.GetCollection<Customer>("customer");
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            if (_customers is null)
            {
                return Enumerable.Empty<Customer>();
            }

            return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer?>> GetById(string id)
        {
            if (_customers is null)
            {
                return NotFound("Customer collection is not initialized.");
            }

            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var targetCustomer = _customers.Find(filter).FirstOrDefault();

            return targetCustomer is not null ? Ok(targetCustomer) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Customer customer)
        {
            if (_customers is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Customer collection is not initialized.");
            }

            if (customer is null)
            {
                return BadRequest("Customer cannot be null");
            }
            else
            {
                await _customers.InsertOneAsync(customer);
                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Customer customer)
        {
            if (_customers is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Customer collection is not initialized.");
            }
            if (customer is null || string.IsNullOrEmpty(customer.Id))
            {
                return BadRequest("Customer or Customer Id cannot be null");
            }
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, customer.Id);

            // First method by using UpdateOneAsync method
            //var update = Builders<Customer>.Update
            //    .Set(x => x.CustomerName, customer.CustomerName)
            //    .Set(x => x.Email, customer.Email);

            //await _customers.UpdateOneAsync(filter, update);

            // Second method by using ReplaceOneAsync method

            var updateResult = await _customers.ReplaceOneAsync(filter, customer);
            return updateResult.IsAcknowledged ? Ok(customer) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (_customers is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Customer collection is not initialized.");
            }
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var deleteResult = await _customers.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0 ? Ok() : NotFound();
        }
    }
}
