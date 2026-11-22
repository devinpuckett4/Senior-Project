using Microsoft.AspNetCore.Mvc;

namespace ChurchOutreachInventory.Controllers;

public class InventoryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}