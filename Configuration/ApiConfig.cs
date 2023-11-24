namespace ApiFuncional.Configuration;

public static class ApiConfig
{
    public static WebApplicationBuilder AddControllersConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Development", builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader());

            options.AddPolicy("Production", builder =>
                builder
                    .WithMethods("POST")
                    .WithOrigins("https://localhost:9000")
                    .AllowAnyHeader());
        });
        return builder;
    }
}