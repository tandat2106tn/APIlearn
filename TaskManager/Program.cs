using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TaskManager.Data;
using TaskManager.Filters;
using TaskManager.Middleware;
using TaskManager.Models;
using TaskManager.Profiles;
using TaskManager.Repositories;
using TaskManager.Services;
using TaskManager.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddControllers();
builder.Services.AddControllers().AddFluentValidation(fv =>
{
	fv.RegisterValidatorsFromAssemblyContaining<CreateTodoTaskDtoValidator>();
	fv.ImplicitlyValidateChildProperties = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagerConnection")));
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "Admin"));
	options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
		options.Events = new JwtBearerEvents
		{
			OnChallenge = context =>
			{
				context.HandleResponse();
				context.Response.StatusCode = 401;
				context.Response.ContentType = "application/json";
				var result = JsonSerializer.Serialize(new { message = "You are not authorized" });
				return context.Response.WriteAsync(result);
			}
		};
	});



builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{


})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();



builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Manager API", Version = "v1" });

	// Add JWT Authentication
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "JWT Authentication",
		Description = "Enter JWT Bearer token **_only_**",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Reference = new OpenApiReference
		{
			Id = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
	{
		new OpenApiSecurityScheme
		{
			Reference = new OpenApiReference
			{
				Type = ReferenceType.SecurityScheme,
				Id = "Bearer"
			},
			Scheme = "oauth2",
			Name = "Bearer",
			In = ParameterLocation.Header,
		},
		new List<string>()
	}
});

	c.OperationFilter<SwaggerAuthorizationOperationFilter>();
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.Use(async (context, next) =>
{
	var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
	if (context.User.Identity.IsAuthenticated)
	{
		logger.LogInformation($"User authenticated: {context.User.Identity.Name}");
		logger.LogInformation($"User roles: {string.Join(", ", context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))}");
	}
	else
	{
		logger.LogInformation("User not authenticated");
	}
	await next();
});
app.UseAuthorization();
app.MapControllers();



using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ApplicationDbContext>();
	var userManager = services.GetRequiredService<UserManager<User>>();
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
	await DataSeeder.SeedData(context, userManager, roleManager);
}

app.Run();
