using PhloSystemAPI.Middleware;
using PhloSystemDomain;
using PhloInfrastructureLayer;
using System.Text.Json;

namespace PhloSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IProductService,ProductService>();
            builder.Services.AddScoped<IProductDataService, ProductDataService>();

            // Add cors to make sure no one access the website.
            builder.Services.AddCors(options => {
                options.AddPolicy("EnableCors", x =>
                {
                    x.AllowAnyOrigin()  //allow only trusted domain 
                     .AllowAnyMethod()  //allow any specific verb if required
                     .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.

            app.MapControllers();

            app.Run();
        }
    }
}
