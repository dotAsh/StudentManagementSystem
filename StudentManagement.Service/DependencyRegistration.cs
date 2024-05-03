using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Persistence.Repository.IRepository;
using StudentManagement.Persistence.Repository;
using StudentManagement.Service.Services.IServices;
using StudentManagement.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;



namespace StudentManagement.Service
{
  
        public static class DependencyRegistration
        {
            public static void RegisterDataAccessDependencies(this IServiceCollection services, string connectionString,bool useInMemoryDatabase)
            {
                services.AddScoped<IStudentRepository, StudentRepository>();

                if (useInMemoryDatabase){
                        services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDatabaseName");
                });
                 }
                else
                {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
                }
            }
        }
    

}
