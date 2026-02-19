using Microsoft.AspNetCore.Mvc;

public class AboutController : Controller
{
    public IActionResult AboutUs()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }
}
