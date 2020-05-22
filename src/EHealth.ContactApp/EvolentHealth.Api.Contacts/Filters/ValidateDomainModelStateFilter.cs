using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Filters
{
	public class ValidateDomainModelStateFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.ModelState.IsValid)
			{
				return;
			}

			var validationErrors = context.ModelState
				.Keys
				.SelectMany(k => context.ModelState[k].Errors)
				.Select(e => e.ErrorMessage)
				.ToArray();

			var json = new {Messages = validationErrors	};

			context.Result = new BadRequestObjectResult(json);
		}
	}
}
