using System.Reflection.Emit;
using System.Data;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformService.Models;
using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {

            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDBContext>(), isProd);
            }
        }

        private static void SeedData(AppDBContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("---> Attemp to apply migration...");
                try
                {
                    //context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
                catch (Exception ex) 
                {
                    
                    Console.WriteLine($"---> Fail to run migration: {ex.Message}");
                }
            }
            if(!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding data ...");
                context.Platforms.AddRange(
                    new Platform() {Name = "Dotnet", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "5$/month"},
                    new Platform() {Name = "Kubernetes", Publisher = "Cloud Native Computing Foudation", Cost = "Free"}
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}