using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;


namespace TodoApi
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    private static string MyAllowSpecificOrigins = "devOrigins";


    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // add services to the DI container
    public void ConfigureServices(IServiceCollection services)
    {
      // services.AddControllers(options =>
      // {
      //     options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
      //     options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
      //     {
      //         ReferenceHandler = ReferenceHandler.Preserve,
      //     }));
      // });
      services.AddHttpContextAccessor();
      services.AddDbContext<TodoDBContext>(opt =>
                      opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
              );



      services.AddScoped<ITodoService, TodoService>();
      services.AddScoped<IAuthService, AuthService>();

      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(o =>
      {
        var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = Configuration["JWT:Issuer"],
          ValidAudience = Configuration["JWT:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Key)
        };
      });

      services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();

      services.AddCors();

      services.AddControllers();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }

    // configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseCors(
          options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
      );

      app.UseRouting();
      app.UseSwagger();
      app.UseSwaggerUI();

      // custom jwt auth middleware
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(x => x.MapControllers());
    }
  }
}