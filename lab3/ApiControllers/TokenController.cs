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
using Microsoft.AspNetCore.Authorization;

namespace lab3.ApiControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Token")]
    [AllowAnonymous]
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

        [HttpGet("RequestTokenVersion")]
        [HttpPost("RequestTokenVersion")]
        [MapToApiVersion("1.0"), MapToApiVersion("1.1")]
        public string GetApiVersion() => HttpContext.GetRequestedApiVersion().ToString();
    }


    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/Token")]
    [AllowAnonymous]
    public class Tokenv1_1Controller : Controller
    {
        private readonly _Context _context;

        public IConfiguration Configuration { get; }

        public Tokenv1_1Controller(IConfiguration configuration, _Context context)
        {
            Configuration = configuration;
            _context = context;
        }

        [HttpPost("RequestToken")]
        public async Task<IActionResult> RequestToken([FromBody] TokenRequestModel tokenRequest)
        {
            var person = await _context.Person.FirstOrDefaultAsync(c => c.PersonType == tokenRequest.PersonType);
            if (person != null)
            {
                JwtSecurityToken token = JwsTokenCreator.CreateToken(tokenRequest.PersonType,
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