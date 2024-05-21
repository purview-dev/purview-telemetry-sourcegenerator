using System.Diagnostics;
using Purview.Telemetry;
using Purview.Telemetry.Activities;
using Purview.Telemetry.Logging;
using Purview.Telemetry.Metrics;

namespace SampleApp.Host.Services;

/* 
 * A multi-target interface that defines
 * the telemetry methods for the WeatherService, including
 * Activities, Events, Logs, and Metrics.
 * 
 * As it's multi-target, each target needs to be
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
	void MinAndMaxReceived(Activity? activity, int minTempInC, int maxTempInC);

	[Log(LogLevel.Warning)]
	void TemperatureOutOfRange(int minTempInC);

	[Log(LogLevel.Warning)]
	void RequestedCountIsTooSmall(int requestCount, int validatedRequestedCount);

	[Counter(AutoIncrement = true)]
	void WeatherForecastRequested();

	[Counter(AutoIncrement = true)]
	void ItsTooCold([Tag] int tooColdCount);

	[Log]
	void TemperatureWithinRange();
}
