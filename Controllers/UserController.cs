using Core.Interfaces;
using Core.Models;
using Core.Models.DTOS;
using Core.Models.Identity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SiteContext _context;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, SiteContext siteContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = siteContext;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
                return NotFound();
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach(var user in users)
            {
                UserDTO userDTO = new UserDTO()
                {
                    DisplayName = user.FullName,
                    Email = user.Email,
                    UserId = user.Id
                };
                userDTOs.Add(userDTO);
            };
            return userDTOs;
        }

        [HttpDelete("Delete User")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            var deleteResult = await _userManager.DeleteAsync(user);
            if (user == null)
                return BadRequest();
            return Ok(deleteResult);
        }

        [HttpPut("Update User")]
        public async Task<ActionResult<UserDTO>> UpdateUser(string Id, UserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return BadRequest("User Not Found");

            user.Email = userDTO.Email;
            await _userManager.UpdateAsync(user);
            return new UserDTO
            {
                Email = user.Email
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> SignIn(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
                return Unauthorized(new ApiResponseType().StatusCode = 401);
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponseType().StatusCode = 401);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_secret_key_123456"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


            //Get Role
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var myToken = new JwtSecurityToken(
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials,
            claims: claims);

            var token = new JwtSecurityTokenHandler().WriteToken(myToken);

            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.FullName,
                UserId = user.Id,
                Token = token,
            };
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var user = new AppUser
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                FullName = registerDTO.FirstName + " " + registerDTO.LastName,
                UserName = registerDTO.FirstName,
                Gender = registerDTO.Gender,
                Email = registerDTO.Email
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponseType().StatusCode = 400);

            UserDTO userDTO= new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.FullName,
                UserId = user.Id,
                Token = "Token Shall Be Generated"
            };
            return Ok(userDTO);
        }

        [HttpGet("TopActiveUser")]
        public async Task<IReadOnlyList<TopUsersDTO>> GetMostActiveUserAsync()
        {
            List<TopUsersDTO> topUsersDTOs = new List<TopUsersDTO>();
            var orderDetails = await _context.ShoppingOrders.GroupBy(SO => SO.UserId).ToListAsync();
            foreach (var item in orderDetails)
            {
                int count = 0;
                foreach (var x in item)
                {
                    count += 1;
                }
                TopUsersDTO topUsersDTO = new TopUsersDTO()
                {
                    Id = item.Key,
                    OrderCount = count
                };
                topUsersDTOs.Add(topUsersDTO);
            }
            return topUsersDTOs;
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ShoppingOrder>> GetOrders(int id)
        //{
        //    var shoppingOrders = await _context.ShoppingOrderProducts.Include(S => S.Product).Include(S => S.ShoppingOrder).Where(S => S.ShoppingOrder.UserId == id).GroupBy(S => S.ShoppingOrder.OrderName).ToListAsync();
        //    List<ShoppingOrderProductDTO> orderProductDTOs = new List<ShoppingOrderProductDTO>();
        //    foreach(var item in shoppingOrders)
        //    {

        //        ShoppingOrderProductDTO orderProductDTO = new ShoppingOrderProductDTO
        //        {
        //            OrderName = item.Key,
        //            Products = new List<string>()
        //        };
        //        foreach (var x in item)
        //        {
        //            orderProductDTO.Products.Add(x.Product.Name);
        //        };
        //        orderProductDTOs.Add(orderProductDTO);
        //    }
        //    return Ok(orderProductDTOs);
        //}
    }
}

            
