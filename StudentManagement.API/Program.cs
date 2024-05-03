
using Microsoft.EntityFrameworkCore;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StudentManagement.API.Middleware;
using StudentManagement.API.Filters;
using StudentManagement.Service.Services;
using StudentManagement.Service.Services.IServices;
using StudentManagement.Service;

namespace StudentManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            //builder.Services.AddControllers();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CentralizedExceptionFilter));
            });

            builder.Services.AddScoped<CentralizedExceptionFilter>();
            builder.Services.AddScoped<CustomAuthorizationFilter>();
           // builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            

            builder.Services.AddHostedService<BackgroundWorkerService>();

            var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
            var connectionString = builder.Configuration.GetConnectionString("DefaultSQLConnection");

            //if (useInMemoryDatabase)
            //{
            //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    {
            //        options.UseInMemoryDatabase("InMemoryDatabaseName");
            //    });
            //}
            //else
            //{
            //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    {
            //        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            //    });
            //}
            builder.Services.RegisterDataAccessDependencies(connectionString, useInMemoryDatabase);

            builder.Services.AddResponseCaching();

            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtIssuer,
                     ValidAudience = jwtIssuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                 };
             });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseRequestHandlerMiddleware();
            app.UseHttpsRedirection();

            // Adding CORS middleware
            app.UseCors("AllowLocalhost");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
