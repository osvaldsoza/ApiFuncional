namespace ApiFuncional.Configuration;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
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
