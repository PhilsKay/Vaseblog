using Microsoft.EntityFrameworkCore;

namespace Blog.Helpers
{
    public static class DataHelper
    {
         public static async Task MaanageDataAsync(IServiceProvider serviceProvider)
        {
            //service: an instance of db context
            var dbContextSvc = serviceProvider.GetRequiredService<DbContext>();

            //Migration: This is the programatic equivalent to Update-Database
            await dbContextSvc.Database.MigrateAsync();
        }
    }
}
