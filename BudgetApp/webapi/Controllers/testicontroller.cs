using Microsoft.AspNetCore.Mvc;

namespace Back_End.Controllers
{
	public class testicontroller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
