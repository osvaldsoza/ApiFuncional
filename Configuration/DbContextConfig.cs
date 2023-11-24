using ApiFuncional.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiFuncional.Configuration;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApiDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        return builder;
    }
}
