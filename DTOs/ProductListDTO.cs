namespace api.DTOs
{
    /// <summary>
    /// Contiene una lista de productos con IDs y cantidades.
    /// </summary>
    public class ProductListDTO
    {
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
    }
}