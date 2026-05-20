namespace api.DTOs
{
    /// <summary>
    /// Clase que representa un producto con su ID y cantidad, 
    /// utilizada para transferir datos entre capas de la aplicación.
    /// </summary>
    public class ProductItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}