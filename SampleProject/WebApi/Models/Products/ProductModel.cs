using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Products
{
    public class ProductModel
    {
        [Required(ErrorMessage = "The product category value is required")]
        [MaxLength(100, ErrorMessage = "The category is too long")]
        public string Category { get; set; }

        [Required(ErrorMessage = "The name value is required")]
        [MaxLength(100, ErrorMessage = "The name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The SKU value is required")]
        [MaxLength(50, ErrorMessage = "The SKU value is too long")]
        public string Sku { get; set; }
        public decimal Price { get; set; }
    }
}