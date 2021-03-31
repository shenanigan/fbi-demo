using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FBI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        public AdminController() { }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            var Claims = new List<Claim>
            {
                new Claim("type", "Admin"),
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

            var Token = new JwtSecurityToken(
                "https://fbi-demo.com",
                "https://fbi-demo.com",
                Claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            );

            return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(Token));
        }


        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "Admin")]
        public IActionResult EnrolAgent([FromBody] Agent Agent)
        {
            var Claims = new List<Claim>
            {
                new Claim("type", "Agent"),
                new Claim("ClearanceLevel", Agent.ClearanceLevel.ToString()),
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

            var Token = new JwtSecurityToken(
                "https://fbi-demo.com",
                "https://fbi-demo.com",
                Claims,
                expires: DateTime.Now.AddDays(30.0),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            );

            return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(Token));
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Agent
    {
        public string Name { get; set; }
        public int ClearanceLevel { get; set; }
    }


}
