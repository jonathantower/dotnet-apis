using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using CR.CreditConnection.Common.Exceptions;
using CR.CreditConnection.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace CR.CreditConnection.Common.Controllers
{
	[Authorize]
	[Route("[controller]")]
	public abstract partial class BaseApiController : Controller
	{
		public IUnitOfWork UnitOfWork { get; }

		protected BaseApiController(IUnitOfWork uow)
		{
			UnitOfWork = uow;
		}

		protected ClaimsPrincipal CurrentUser => User;

		public string CurrentUserRole => GetClaim<string>(@"http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
		public long CurrentUserId => GetClaim<long>("UserId");
		public string CurrentUsername => GetClaim<string>("Username");
        public long CurrentTenantId => GetClaim<long>("TenantId");

		[NonAction]
		public IActionResult ValidationError(string error)
		{
			return BadRequest(new ErrorResponseDTO
			{
				GeneralErrors = new[] { error }
			});
		}

		[NonAction]
		public IActionResult ValidationError(ModelStateDictionary modelState, params string[] errors)
		{
			return BadRequest(new ErrorResponseDTO
			{
				GeneralErrors = errors,
				FieldErrors = modelState.Select(i => new FieldError
				{
					FieldName = i.Key,
					Errors = i.Value.Errors.Select(e => e.ErrorMessage).ToArray()
				}).ToArray()
			});
		}

		[NonAction]
		public IActionResult ValidationError(params string[] errors)
		{
			return BadRequest(new ErrorResponseDTO
			{
				GeneralErrors = errors,
			});
		}

		[NonAction]
		public IActionResult ValidationError()
		{
			return BadRequest(new ErrorResponseDTO());
		}

		protected T GetClaim<T>(string claimName)
		{
			var user = CurrentUser;
			if (user == null) return default(T);

			var c = CurrentUser.Claims.FirstOrDefault(i => string.Compare(i.Type, claimName.ToLower(), StringComparison.OrdinalIgnoreCase) == 0);
			if (c == null) return default(T);

			return (T)Convert.ChangeType(c.Value, typeof (T));
		}

		protected void Respond(HttpStatusCode code, string message)
		{
			throw new HttpResponseException(code, message);
		}

		protected void Error(Exception ex)
		{
			var e = ex;
			while (e.InnerException != null) e = e.InnerException;

			Respond(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(new
			{
				Error = e.Message
			}));
		}
	}
}
