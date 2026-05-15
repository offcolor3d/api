using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    /// <summary>
    /// Clase que representa un producto en la aplicación.
    /// </summary>
    public class Product
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public required string Title { get; set; }

        [Column("description")]
        public required string Description { get; set; }

        [Column("images_url")]
        public required string[] ImagesUrl { get; set; }

        [Column("material")]
        public required string Material { get; set; }

        [Column("size")]
        public required int[] Size { get; set; }

        [Column("price")]
        public required decimal Price { get; set; }
    }
}