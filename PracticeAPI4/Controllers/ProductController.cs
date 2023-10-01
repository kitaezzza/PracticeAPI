using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeAPI4.DB;
using PracticeAPI4.Models;

namespace PracticeAPI4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly APIContext _context;
        public ProductController(APIContext context)
        {
            _context = context;
        }

        [HttpGet("GETALLPRODUCTS")]
        public IActionResult GetAllProducts([FromHeader] string authtoken)
        {
            try
            {
                #region Validation
                if (string.IsNullOrWhiteSpace(authtoken))
                    return NotFound("Token was not found.");

                var usertoken = _context.Users.Select(t => t.Token == authtoken);
                if (usertoken == null)
                    return Unauthorized("Uncorrect token.");
                #endregion

                var products = _context.Products.ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GETPRODUCT/{id}")]
        public IActionResult GetProduct([FromHeader] string authtoken, int id)
        {
            try
            {
                #region Validation
                if (string.IsNullOrWhiteSpace(authtoken))
                    return NotFound("Token was not found.");

                var usertoken = _context.Users.Select(t => t.Token == authtoken);
                if (usertoken == null)
                    return Unauthorized("Uncorrect token.");
                #endregion

                var product = _context.Products.Find(id);
                if (product == null)
                    return NotFound("Product was not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("ADDPRODUCT")]
        public IActionResult AddProduct([FromHeader] string authtoken,
            [FromBody] Product product)
        {
            try
            {
                #region Validation
                if (string.IsNullOrWhiteSpace(authtoken))
                    return NotFound("Token was not found.");

                var usertoken = _context.Users.Select(t => t.Token == authtoken);
                if (usertoken == null)
                    return Unauthorized("Uncorrect token.");
                #endregion

                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok("This product was added successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("EDITPRODUCT")]
        public IActionResult EditProduct([FromHeader] string authtoken,
            [FromBody] Product product)
        {
            try
            {
                #region Validation
                if (string.IsNullOrWhiteSpace(authtoken))
                    return NotFound("Token was not found.");

                var usertoken = _context.Users.Select(t => t.Token == authtoken);
                if (usertoken == null)
                    return Unauthorized("Uncorrect token.");
                #endregion

                var existingproduct = _context.Products.FirstOrDefault(p => p.ID == product.ID);
                if (existingproduct == null)
                    return NotFound("Product was not found");
                _context.Entry(existingproduct).CurrentValues.SetValues(product);
                _context.SaveChanges();
                return Ok("This product was updated successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("DELETEPRODUCT/{id}")]
        public IActionResult DeleteProduct([FromHeader] string authtoken, int id)
        {
            try
            {
                #region Validation
                if (string.IsNullOrWhiteSpace(authtoken))
                    return NotFound("Token was not found.");

                var usertoken = _context.Users.Select(t => t.Token == authtoken);
                if (usertoken == null)
                    return Unauthorized("Uncorrect token.");
                #endregion

                var product = _context.Products.Find(id);
                if (product == null)
                    return NotFound("Product was not found");

                _context.Remove(product);
                _context.SaveChanges();
                return Ok("Product was removed successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
