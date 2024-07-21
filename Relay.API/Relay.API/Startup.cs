using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

namespace Relay.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services
         .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
           options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
           options.Audience = Configuration["Auth0:Audience"];
           options.TokenValidationParameters = new TokenValidationParameters
           {
             NameClaimType = ClaimTypes.NameIdentifier
           };
         });

      services.AddAuthorization(options =>
      {
        options.AddPolicy(
          "read:messages",
          policy => policy.Requirements.Add(
            new HasScopeRequirement("read:messages, read:admin", "https://auth.bittershark.com/")
          )
        );
      });

      services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

      services.AddControllers().AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
      });

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Relay.Customers.API", Version = "v1" });
      });

      services.AddCors(options =>
      {
        options.AddPolicy(name: "CorsPolicy", builder =>
        {
          builder.WithOrigins("*")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
      });

      Container.Install();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      //if (env.IsDevelopment())
      //{
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Relay.API v1"));
      //}

      app.UseHttpsRedirection();

      app.UseAuthentication();
      app.UseRouting();

      app.UseCors();

      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
