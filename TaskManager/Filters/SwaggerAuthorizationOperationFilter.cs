using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManager.Filters
{
	public class SwaggerAuthorizationOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
				.Union(context.MethodInfo.GetCustomAttributes(true))
				.OfType<AuthorizeAttribute>();

			if (authAttributes.Any())
			{
				operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
				operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

				var securityRequirement = new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] {}
				}
			};
				operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
			}
		}
	}
}