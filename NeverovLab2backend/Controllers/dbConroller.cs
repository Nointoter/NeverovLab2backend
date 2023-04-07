using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;


namespace NeverovLab2backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class dbController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> ReloadDb()
        {
            using (var db = new pgDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            return Ok();
        }

    }
}