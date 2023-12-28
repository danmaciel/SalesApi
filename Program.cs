using Microsoft.EntityFrameworkCore;

using System.Reflection;
using Microsoft.OpenApi.Models;
using SalesApi.src.Data;
using SalesApi.src.Services;
using Microsoft.AspNetCore.Identity;
using SalesApi.src.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SalesContext>(
   options => options
    .UseLazyLoadingProxies()
    .UseSqlite(
        builder.Configuration.GetConnectionString("WebApiDatabase")
    )
);

builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SalesContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDebtervice, DebtService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { 
             Name = "Authorization", 
            Type = SecuritySchemeType.ApiKey, 
            Scheme = "Bearer", 
            BearerFormat = "JWT", 
            In = ParameterLocation.Header, 
            Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"", 
        }); 

        c.AddSecurityRequirement(new OpenApiSecurityRequirement { 
            { 
                new OpenApiSecurityScheme { 
                    Reference = new OpenApiReference { 
                        Type = ReferenceType.SecurityScheme, 
                                  Id = "Bearer" 
                    } 
                }, 
                new string[] {} 
            } 
        }); 
   }
);

builder.Services.AddAuthentication(
    options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer( 
    options => {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey= true,
            //IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Aqweq012010102AAcczafghhsdsderdasda")),
            IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SymmetricSecurityKey")["value"]!)),
            ClockSkew= TimeSpan.Zero,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

DatabaseManagementService.MigrationInitialisation(app);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
