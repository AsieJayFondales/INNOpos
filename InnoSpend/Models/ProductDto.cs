// Purpose: This file contains the ProductDto class, which is used to update and create products.
// It contains the properties Name, Brand, Category, Price, Description, and ImageFile.
// The ImageFile property is an IFormFile object, which is used to store the image file of the product.
// The other properties are used to store the name, brand, category, price, and description of the product.
// The class is used to transfer data between the client and the server when creating or updating products.
// The class is used in the ProductController to handle the creation and updating of products.

using System.ComponentModel.DataAnnotations;

namespace InnoSpend.Models
{ 
    public class ProductDto 
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required, MaxLength(100)]
        public string Brand { get; set; } = "";

        [Required, MaxLength(100)]
        public string Category { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; } = "";

        public IFormFile? ImageFile { get; set; } //required if create, optional if update
    }
}
