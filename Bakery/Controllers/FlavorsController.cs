using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Bakery.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
  public class FlavorsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorsController(UserManager<ApplicationUser> userManager, BakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      ViewBag.PageTitle = "View all flavors";
      List<Flavor> flavorList = _db.Flavors.Include(flavor => flavor.JoinEntities)
          .ThenInclude(join => join.Treat).OrderByDescending(flavor => flavor.JoinEntities.Count).ToList();

      return View(flavorList);
    }

    public ActionResult Details(int id)
    {
      Flavor thisFlavor = _db.Flavors
          .Include(flavor => flavor.JoinEntities)
          .ThenInclude(join => join.Treat)
          .FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.PageTitle = $"{thisFlavor.Name} Details";
      return View(thisFlavor);
    }
    [Authorize]
    public ActionResult Create()
    {
      ViewBag.PageTitle = "Add a flavor";
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult Create(Flavor flavor)
    {
      ViewBag.PageTitle = "flavors";
      if (!ModelState.IsValid)
      {
        return View(flavor);
      }
      else
      {
      _db.Flavors.Add(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
      }
    }

    [Authorize]
    public ActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors
      .Include(flavor => flavor.JoinEntities)
      .ThenInclude(join => join.Treat)
      .FirstOrDefault(flavor => flavor.FlavorId == id);

      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      ViewBag.PageTitle = $"Add a treat to {thisFlavor.Name}'s profile";
      return View(thisFlavor);
    }

    [Authorize]
    [HttpPost]
    public ActionResult AddTreat(Flavor flavor, int treatId)
    {
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.TreatId == treatId && join.FlavorId == flavor.FlavorId));
      #nullable disable
      if (joinEntity == null && treatId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { TreatId = treatId, FlavorId = flavor.FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = flavor.FlavorId });
    }  

    [Authorize]
    public ActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavors => flavors.FlavorId == id);
      ViewBag.PageTitle = $"Modify {thisFlavor.Name}";
      return View(thisFlavor);
    }
    
    [Authorize]
    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]    
    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.PageTitle = $"Delete {thisFlavor.Name}";
      return View(thisFlavor);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
      _db.FlavorTreats.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
