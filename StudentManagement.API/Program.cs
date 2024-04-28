
using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Data;
using StudentManagement.Persistence.Repository.IRepository;
using StudentManagement.Persistence.Repository;


namespace StudentManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));


            var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

            if (useInMemoryDatabase)
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDatabaseName");
                });
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
                });
            }
            
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

            app.UseHttpsRedirection();

            // Adding CORS middleware
            app.UseCors("AllowLocalhost");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
