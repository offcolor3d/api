using api.Data;
using api.DTOs;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products?category=someSubcategory&subcategory=someSubcategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? category, 
        [FromQuery] string? subcategory)
        {
            //Creamos una consulta base que se puede ir filtrando según los parametros proporcionados
            var queryParams = _context.Products.AsQueryable();

            //Agrega el filtro de categoria si se proporciona
            if(!string.IsNullOrEmpty(category))
            {
                queryParams = queryParams.Where(p => p.Category.ToLower() == category.ToLower());
            }

            //Agrega el filtro de subcategoria si se proporciona
            if(!string.IsNullOrEmpty(subcategory))
            {
                queryParams = queryParams.Where(p => p.Subcategory.ToLower() == subcategory.ToLower());
            }

            //Ejecuta la consulta con los filtros aplicados y devuelve los resultados
            var filteredProducts = await queryParams.ToListAsync();

            if(!filteredProducts.Any())
            {
                Console.WriteLine("No se encontraron productos que coincidan " + 
                "con los filtros proporcionados. \nCategoria: " + category + "\nSubcategoria: " + subcategory);
                Console.WriteLine("Mandando todos los productos sin filtrar...");
                filteredProducts = await _context.Products.ToListAsync();
            }

            return Ok(filteredProducts);
        }
        

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // GET: api/products/priceof
        [HttpPost("priceof")]
        public async Task<ActionResult<decimal>> GetPriceOf([FromBody] ProductListDTO productList)
        {
            //Si la lista es nula o esta vacia se manda 0.
            if (productList?.Products == null || !productList.Products.Any())
                return Ok(0);

            //Extrae todos los ID del DTO.
            var productIds = productList.Products.Select(p => p.Id).ToList();

            //Recoge todos los productos de la base de datos que coincidan con esos IDs.
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            //Calcula el precio total multiplicando por la cantidad.
            decimal total = 0;
            foreach (var item in productList.Products)
            {
                var dbProduct = products.FirstOrDefault(p => p.Id == item.Id);
                if (dbProduct != null)
                {
                    total += dbProduct.Price * item.Quantity;
                }
            }

            return Ok(total);
        }
    }
}