namespace SampleApp.Host.Services;

public interface IWeatherService
{
	IEnumerable<WeatherForecast> GetWeatherForecastsAsync(int requestCount, CancellationToken cancellationToken = default);
}
