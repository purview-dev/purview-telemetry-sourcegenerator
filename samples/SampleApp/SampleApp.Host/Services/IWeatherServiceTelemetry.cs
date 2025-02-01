using System.Diagnostics;
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;

namespace SampleApp.Host.Services;

/* 
 * A multi-target interface that defines
 * the telemetry methods for the WeatherService, including
 * Activities, Events, Logs, and Metrics.
 * 
 * As it's multi-target, each target (method) needs to be
 * explicitly defined.
*/

[ActivitySource]
[Logger]
[Meter]
public interface IWeatherServiceTelemetry
{
	[Activity(ActivityKind.Client)]
	Activity? GettingWeatherForecastFromUpstreamService([Baggage] string someRandomBaggageInfo, int requestedCount, int validatedRequestedCount);

	[Event]
	void ForecastReceived(Activity? activity, int minTempInC, int maxTempInC);

	[Event]
	void FailedToRetrieveForecast(Activity? activity, Exception ex);

	[AutoCounter]
	void WeatherForecastRequested();

	[AutoCounter]
	void ItsTooCold(int tooColdCount);

	[Log(LogLevel.Warning)]
	void TemperatureOutOfRange(int minTempInC);

	[Warning]
	void RequestedCountIsTooSmall(int requestCount, int validatedRequestedCount);

	[Info]
	void TemperatureWithinRange();

	[Error]
	void WeatherForecastRequestFailed(Exception ex);
}
