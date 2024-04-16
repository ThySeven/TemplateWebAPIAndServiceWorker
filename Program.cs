
namespace Planning_Service;

public class Program
{
    public static void Main(string[] args)
    {
        var webApplicationOptions = new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                ? AppContext.BaseDirectory
                : default
        };

        var builder = WebApplication.CreateBuilder(webApplicationOptions);

        // Add services to the container.
        builder.Services.Configure<DeliveryDatabaseSettings>(
            builder.Configuration.GetSection("DeliveryDatabase"));

        builder.Services.AddSingleton<DeliveryService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHostedService<Worker>();
        builder.Host.UseWindowsService();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
