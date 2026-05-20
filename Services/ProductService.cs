using api.Data;
using api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    /// <summary>
    /// Clase que implementa la logica de negocio relacionada con los productos,
    /// </summary>
    public class ProductService : IProductService
    {
        public async Task<decimal> CalculateTotalPriceAsync(ProductListDTO productList, ApplicationDbContext context)
        {
            // Validamos que los parametros no sean nulos, 
            // en caso de serlo se manda una excepcion para que el 
            // controlador pueda manejarla y devolver un error 400 al cliente.
            if (productList == null || context == null)
            {
                throw new ArgumentNullException();
            }

            //Si la lista esta vacia es 0.
            if (!productList.Products.Any()) return 0;

            //Extrae todos los IDs en una lista.
            var productIds = productList.Products.Select(p => p.Id).ToList();

            //Obtiene todos los productos de la base de datos.
            var products = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            decimal total = 0;

            //Suma el precio de cada producto multiplicado por su cantidad.
            foreach (var item in productList.Products)
            {
                var product = products.FirstOrDefault(p => p.Id == item.Id);
                if (product != null)
                {
                    total += product.Price * item.Quantity;
                }
            }

            return total;
        }
    }
}