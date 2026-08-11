using Asp.Versioning;
using JobAggregator.Application.Features.WeatherForecast.Queries;
using JobAggregator.Contracts.Common;
using JobAggregator.Contracts.Weather;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobAggregator.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("[controller]")]
public class WeatherForecastController(ISender sender) : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult<IReadOnlyCollection<WeatherForecastDto>>> Get(
        [FromQuery] int days = 5,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetWeatherForecastQuery(days), cancellationToken);
        return Ok(result.Items);
    }

    [HttpGet("paged", Name = "GetWeatherForecastPaged")]
    public async Task<ActionResult<ApiResponse<PagedResult<WeatherForecastDto>>>> GetPaged(
        [FromQuery] int days = 30,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetWeatherForecastQuery(days, pageNumber, pageSize), cancellationToken);
        return Ok(ApiResponse<PagedResult<WeatherForecastDto>>.Ok(result));
    }
}
