using Microsoft.EntityFrameworkCore;
namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                System.Console.WriteLine("--> Attempting to apply migrations ...");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Couldn't run migrations: {ex.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                System.Console.WriteLine("Seeding data ...");
                context.Platforms.AddRange(
                    new Models.Platform()
                    {
                        Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"
                    },
                     new Models.Platform()
                    {
                        Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free"
                    },
                     new Models.Platform()
                    {
                        Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"
                    }
                );
                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data");
            }
        }
    }
}