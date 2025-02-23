using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace InnoSpend.Views.Products
{
    public class CategoriesModel : PageModel
    {
        public void OnGet()
        {
            // Example list of categories
            var categories = new List<Category>
        {
            new Category { Name = "Electronics" },
            new Category { Name = "Books" },
            new Category { Name = "Clothing" }
        };

            ViewData["Categories"] = categories;
        }
    }

    public class Category
    {
        public string Name { get; set; } = "";
    }
}

