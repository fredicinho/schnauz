using Schnauz.QueryHandlers.Core;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Queries;

namespace Schnauz.QueryHandlers;
public class WeatherQueryHandler : QueryHandlerBase<WeatherQuery, WeatherDto[]>
{
    protected override Task<WeatherDto[]> Execute(WeatherQuery query)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherDto
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray());
    }
}
