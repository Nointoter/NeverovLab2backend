using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverovLab2backend.Data;
using NeverovLab2backend.Models;


namespace NeverovLab2backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController: Controller
    {
        private readonly pgDbContext _pgDbContext;

        public MembersController(pgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var members = await _pgDbContext.Members.ToListAsync();
            return Ok(members);
        }
        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] Member memberRequest)
        {
            using (var pgDbContext = new pgDbContext())
            {
                memberRequest.Id = Guid.NewGuid();
                pgDbContext.Members.Add(memberRequest);
                pgDbContext.SaveChangesAsync();               
            }               
            return Ok(memberRequest);
        }

    }   
}