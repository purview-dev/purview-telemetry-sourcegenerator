namespace SampleApp.Host.Services;

sealed class WeatherService(IWeatherServiceTelemetry telemetry) : IWeatherService
{
	const int _tooColdTempInC = -10;

	static readonly string[] _summaries =
	[
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	];

	public IEnumerable<WeatherForecast> GetWeatherForecastsAsync(int requestCount, CancellationToken cancellationToken = default)
	{
		var validatedRequestedCount = requestCount;
		if (validatedRequestedCount < 5)
		{
			validatedRequestedCount = 5;

			telemetry.RequestedCountIsTooSmall(requestCount, validatedRequestedCount);
		}

		using var activity = telemetry.GettingWeatherForecastFromUpstreamService($"{Guid.NewGuid()}",
			requestCount,
			validatedRequestedCount);

		telemetry.WeatherForecastRequested();

		// This would usually be async of course...
		cancellationToken.ThrowIfCancellationRequested();

		var results = Enumerable.Range(1, validatedRequestedCount).Select(index => new WeatherForecast
		{
			Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
			TemperatureC = Random.Shared.Next(-20, 55),
			Summary = _summaries[Random.Shared.Next(_summaries.Length)]
		}).ToArray();

		var minTempInC = results.Min(m => m.TemperatureC);
		telemetry.MinAndMaxReceived(activity,
			minTempInC,
			results.Max(wf => wf.TemperatureC)
		);

		if (minTempInC < _tooColdTempInC)
		{
			telemetry.TemperatureOutOfRange(minTempInC);
			telemetry.ItsTooCold(results.Count(wf => wf.TemperatureC < _tooColdTempInC));
		}
		else
			telemetry.TemperatureWithinRange();

		return results;
	}
}
