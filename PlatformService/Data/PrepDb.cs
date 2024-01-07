using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {


            using (var ServiceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(ServiceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }


        private static void SeedData(AppDbContext context, bool IsProd){

            if(IsProd)
            {
                Console.WriteLine(" ==> APPLYING MIGRATION...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(" ==> COULD NOT RUN MIGRATION... " + ex.Message);
                }
            }

            if(!context.Platforms.Any()){

                Console.WriteLine(" ==> Seeding data.");
                context.Platforms.AddRange(
                    new Platform(){Name = "Dotnet", Publisher="Microsoft", Cost="Free"},
                    new Platform(){Name = "SQL Server Express", Publisher="Microsoft", Cost="Free"},
                    new Platform(){Name = "Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
                );

                context.SaveChanges();

            }
            else
            {
                Console.WriteLine(" ==> We already have data.");
            }
        }

    }




}