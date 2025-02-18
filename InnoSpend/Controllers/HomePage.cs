using Microsoft.AspNetCore.Mvc; 

namespace InnoSpend.Views.Home
{
    public class HomePage : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }

    public class InputModel
    {
        public required string Username { get; set; }
    }

}
