using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet("Get Roles")]
        public async Task<ActionResult> GetAllRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            List<RoleDTO> roleDTOs = new List<RoleDTO>();
            if (roles.Count != 0) return Ok(roles);
            else return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Add(string? Name)
        {
            if (Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return BadRequest();
            }
            IdentityRole role = new IdentityRole(Name);
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok(role);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest();
        }
    }
}
