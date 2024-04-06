namespace SampleApp.Host.Interfaces.Services; 

public interface IWeatherService {
	IEnumerable<WeatherForecast> GetWeatherForecastsAsync(int requestCount, CancellationToken cancellationToken = default);
}
