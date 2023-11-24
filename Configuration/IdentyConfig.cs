using System.Text;
using ApiFuncional.Data;
using ApiFuncional.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApiFuncional.Configuration;

public static class IdentyConfig
{
    public static WebApplicationBuilder AddIdentyConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApiDbContext>();

//pegando o token gerando a chaveencodada
        var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(JwtSettingsSection);

        var jwtSettings = JwtSettingsSection.Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audiencia,
                ValidIssuer = jwtSettings.Emissor
            };
        });

        return builder;
    }
}