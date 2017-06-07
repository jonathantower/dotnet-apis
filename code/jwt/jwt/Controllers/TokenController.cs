using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwt.Controllers
{
	[Route("api/token")]
    public class TokenController : Controller
    {
		[HttpGet]
	    public IActionResult GetToken()
	    {
			var handler = new JwtSecurityTokenHandler();
			var tokenExpirationMinutes = 30;
			var expires = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes);

			var identity = new ClaimsIdentity(new GenericIdentity("username"), new[]
			{
				new Claim("userid", "1", ClaimValueTypes.Integer),
			}, "", "", "");

			var cred = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my key for authentication")), 
				SecurityAlgorithms.HmacSha256);

			var securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = "Issuer",
				Audience = "Audience",
				SigningCredentials = cred,
				Subject = identity,
				Expires = expires
			});

			return Ok(new 
			{
				Token = handler.WriteToken(securityToken)
			});
		}
    }
}
