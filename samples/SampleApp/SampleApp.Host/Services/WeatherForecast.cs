namespace SampleApp.Host.Services;

readonly public record struct WeatherForecast(
	DateOnly Date,
	int TemperatureC,
	string? Summary)
{

	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
