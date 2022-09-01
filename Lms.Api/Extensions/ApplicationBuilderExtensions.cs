using Lms.Data.Data;

namespace Lms.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<LmsApiContext>();

                //db.Database.EnsureDeleted();
                //db.Database.Migrate();

                //dotnet user-secrets set "AdminPW" "BytMig123!"
                //var config = serviceProvider.GetRequiredService<IConfiguration>();
                //var adminPW = config["AdminPW"];

                try
                {
                    await SeedData.InitAsync(db, serviceProvider);
                }
                catch (Exception e)
                {

                    throw;
                }
            }

            return app;

        }
    }
}
