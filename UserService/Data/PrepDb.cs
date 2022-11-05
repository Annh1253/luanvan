using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {

            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DBContext>(), true);
            }
        }

        private static void SeedData(DBContext context, bool isProd)
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
            if(!context.Users.Any())
            {
                Console.WriteLine("---> Seeding data ...");
                Role adminRole = new Role(){Name = "Admin"};
                Role contestantRole = new Role(){Name = "Contestant"};
                context.Roles.AddRange(
                    adminRole,
                    contestantRole
                );

                context.Users.AddRange(
                  new User(){Email = "anhanh1253@gmail.com", FirstName = "Nguyen Lam", LastName = "Hoang Anh", Password = "password", Roles = new List<Role>(){adminRole}},
                  new User(){Email = "emem1253@gmail.com", FirstName = "Nguyen Lam", LastName = "Hoang Em", Password = "password", Roles = new List<Role>(){contestantRole}}
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