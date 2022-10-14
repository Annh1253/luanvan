using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandService.Contracts;
using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPoppulation(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();
                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
            }

            

        }
        private static void SeedData(ICommandRepository commandRepository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("---> Seeding new platforms...");
            foreach(var platform in platforms)
            {
                if(!commandRepository.ExternalPlatformExist(platform.ExternalId))
                {
                    Console.WriteLine();
                    commandRepository.CreatePlatform(platform);

                }
                commandRepository.SaveChanges(); 
                
            }
        }
    }
}