using SampleApp.Host.Services;

namespace SampleApp.Host;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.AddServiceDefaults();
		builder.Services.AddMetrics();

		// Add services to the container.

		// This is a generated method that adds the
		// IWeatherServiceTelemetry interface to the container
		// as a singleton.
		builder.Services.AddWeatherServiceTelemetry();

		builder.Services.AddTransient<IWeatherService, WeatherService>();

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		app.MapDefaultEndpoints();

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
