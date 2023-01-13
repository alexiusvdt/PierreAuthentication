using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {
      private readonly BakeryContext _db;
      private readonly UserManager<ApplicationUser> _userManager;

      public HomeController(UserManager<ApplicationUser> userManager, BakeryContext db)
      {
        _userManager = userManager;
        _db = db;
      }

      [HttpGet("/")]
      public ActionResult Index()
      {
        Flavor[] flavors = _db.Flavors.ToArray();
        Treat[] treats = _db.Treats.ToArray();
        Dictionary<string, object[]> model = new Dictionary<string, object[]>();
        model.Add("flavors", flavors);
        model.Add("treats", treats);
        return View(model);
      }
    }
}