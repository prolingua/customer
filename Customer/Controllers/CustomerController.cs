using System.Linq;
using Customer.EF;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerDb _customberDb;
        public CustomerController(ICustomerDb customerDb)
        {
            _customberDb = customerDb;
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var customer =  _customberDb.Customers.SingleOrDefault(c => c.Id == id);
            
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_customberDb.Customers);
        }

        [HttpPost]
        public IActionResult Create([FromBody] EF.Customer newCustomer)
        {
            return Ok(_customberDb.AddCustomer(newCustomer));
        }

        [HttpPut]
        public IActionResult Update([FromBody] EF.Customer customer)
        {
            customer = _customberDb.UpdateCustomer(customer);            
            
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var customer = _customberDb.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_customberDb.DeleteCustomer(customer) == 1);
        }
    }
}