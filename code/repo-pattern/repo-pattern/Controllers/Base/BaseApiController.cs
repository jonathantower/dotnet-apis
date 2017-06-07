using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.CreditConnection.Common.Controllers
{
	[Authorize]
	[Route("[controller]")]
	public abstract class BaseApiController : Controller
	{
		public long CurrentUserId => GetClaim<long>("UserId");
		public string CurrentUsername => GetClaim<string>("Username");
		protected ClaimsPrincipal CurrentUser => User;

		protected T GetClaim<T>(string claimName)
		{
			var user = CurrentUser;
			if (user == null) return default(T);

			var c = CurrentUser.Claims.FirstOrDefault(i => string.Compare(i.Type, claimName.ToLower(), StringComparison.OrdinalIgnoreCase) == 0);
			if (c == null) return default(T);

			return (T)Convert.ChangeType(c.Value, typeof (T));
		}
	}
}
