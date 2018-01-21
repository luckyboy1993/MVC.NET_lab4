using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DALlab3.Entities;
using System.IdentityModel.Tokens.Jwt;
using lab3.Models;
using Microsoft.Extensions.Configuration;

namespace lab3.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private readonly _Context _context;

        public IConfiguration Configuration { get; }

        public TokenController(IConfiguration configuration, _Context context)
        {
            Configuration = configuration;
            _context = context;
        }

        [HttpPost("RequestToken")]
        public IActionResult RequestToken([FromBody] TokenRequestModel tokenRequest)
        {
            if (_context.Customer.Any(c => c.Person.FirstName == tokenRequest.FirstName
                && c.Person.PersonPhone.Any(p => p.PhoneNumber == tokenRequest.Phone)))
            {
                JwtSecurityToken token = JwsTokenCreator.CreateToken(tokenRequest.FirstName,
                    Configuration["Auth:JwtSecurityKey"],
                    Configuration["Auth:ValidIssuer"],
                    Configuration["Auth:ValidAudience"]);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenStr);
            }
            return Unauthorized();
        }
        

    }
}