using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Profiles;
using TaskManager.Repositories;
using TaskManager.Services;
using TaskManager.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddDebug();
builder.Services.AddControllers().AddFluentValidation(fv =>
{
	fv.RegisterValidatorsFromAssemblyContaining<CreateTodoTaskDtoValidator>();
	fv.ImplicitlyValidateChildProperties = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Manager API", Version = "v1" });

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
	});
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagerConnection")));

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Register repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddScoped<JwtService>();

// Configure authorization
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "Admin"));
	options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// Configure JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings.Issuer,
		ValidAudience = jwtSettings.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
	};
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder => builder
			.WithOrigins("https://localhost:7277")
			.AllowAnyHeader()
			.AllowCredentials());
});


// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
	// You can configure identity options here if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

// Logging middleware
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

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ApplicationDbContext>();
	var userManager = services.GetRequiredService<UserManager<User>>();
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
	await DataSeeder.SeedData(context, userManager, roleManager);
}

// Configure CORS
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();