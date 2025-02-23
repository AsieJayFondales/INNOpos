using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace InnoSpend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";


        public IActionResult OnPost()
        {
            return new JsonResult(new { success = true });
        }
    }
}
