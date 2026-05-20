using api.Data;
using api.DTOs;

namespace api.Services
{
    /// <summary>
    /// Interfaz que define los métodos para gestionar productos en la aplicación
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Calcula el precio total de una lista de productos, 
        /// teniendo en cuenta las cantidades y los precios individuales.
        /// </summary>
        /// <param name="productList"></param>
        /// <returns></returns>
        Task<decimal> CalculateTotalPriceAsync(ProductListDTO productList, ApplicationDbContext context);
    }
}