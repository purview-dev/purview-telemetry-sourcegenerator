using Microsoft.AspNetCore.Mvc;
using SampleApp.Host.Interfaces.Services;

namespace SampleApp.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IWeatherService service) : ControllerBase
{
	[HttpGet(Name = "GetWeatherForecast")]
	public IEnumerable<WeatherForecast> Get() 
		=> service.GetWeatherForecastsAsync(5);

	[HttpGet("{requestCount}", Name = "GetWeatherForecastWithRequest")]
	public IEnumerable<WeatherForecast> Get(int requestCount) 
		=> service.GetWeatherForecastsAsync(requestCount);
}
