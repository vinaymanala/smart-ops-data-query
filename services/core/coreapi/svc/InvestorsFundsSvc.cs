using core.data.repositories;
using Microsoft.AspNetCore.Mvc;

namespace core.coreapi.svc;

public interface IInvestorsFundsSvc
{
    Task<IResult> _InternalApiGetInvestorDataByIdAsync(string searchId);
    Task<IResult> _InternalApiGetFundDataByIdAsync(string searchId);
    Task<IResult> _InternalApiGetInvestorDataByRegionAsync(string region);
    Task<IResult> _InternalApiGetInvestorDataByCountryAsync(string country);
    Task<IResult> _InternalApiGetInvestorDataByCityAsync(string city);
    // Task<IResult> _InternalApiGetInvestorsDataAsync(InvestorDto investorDto);
    // Task<IResult> _InternalApiGetFundsDataAsync(FundDto fundDto);
}
public class InvestorsFundsSvc(
    IInvestosFundsRepository repository,
    ILogger<InvestorsFundsSvc> logger) : IInvestorsFundsSvc
{
    private readonly IInvestosFundsRepository _repo = repository;

    public async Task<IResult> _InternalApiGetInvestorDataByIdAsync([FromQuery(Name = "searchId")] string searchId)
    {        
        if (string.IsNullOrEmpty(searchId))
        {
            logger.LogError("No Data returned. Check the payload again.");
            return Results.BadRequest(new { error = "searchId is required" });
        }
        try
        {
            var data = await _repo.GetInvestorByIdFromDBAsync(searchId: searchId);
            if (data is null)
            {
                return Results.NotFound(new { error = $"Investor with id '{searchId}' was not found." });
            }

            return Results.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving investor by id: {SearchId}", searchId);
            return Results.Json(new { error = ex.Message }, statusCode: 500);
        }
    }

    public async Task<IResult> _InternalApiGetFundDataByIdAsync([FromQuery(Name = "searchId")] string searchId)
    {
        if (string.IsNullOrEmpty(searchId))
        {
            logger.LogError("No Data returned. Check the payload again.");
            return Results.BadRequest(new { error = "searchId is required" });
        }
        try
        {
            var data = await _repo.GetFundByIdFromDBAsync(searchId: searchId);
            if (data is null)
            {
                return Results.NotFound(new { error = $"Fund with id '{searchId}' was not found." });
            }

            return Results.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving fund by id: {SearchId}", searchId);
            return Results.Json(new { error = ex.Message }, statusCode: 500);
        }
    }

    public async Task<IResult> _InternalApiGetInvestorDataByRegionAsync([FromQuery(Name = "region")] string region)
    {
        if (string.IsNullOrEmpty(region))
        {
            logger.LogError("No Data returned. Check the payload again.");
            return Results.BadRequest(new { error = "region is required" });
        }
        try
        {
            var data = await _repo.GetInvestorByRegionFromDBAsync(region: region);
            if (data is null)
            {
                return Results.NotFound(new { error = $"Investor with '{region}' was not found." });
            }
            return Results.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving investor: {region}");
            return Results.Json(new { error = ex.Message }, statusCode: 500);
        }
    }

    public async Task<IResult> _InternalApiGetInvestorDataByCountryAsync([FromQuery(Name = "country")] string country)
    {
        if (string.IsNullOrEmpty(country))
        {
            logger.LogError("No Data returned. Check the payload again.");
            return Results.BadRequest(new { error = "country is required" });
        }
        try
        {
            var data = await _repo.GetInvestorByCountryFromDBAsync(country: country);
            if (data is null)
            {
                return Results.NotFound(new { error = $"Investor with '{country}' was not found." });
            }
            return Results.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving investor: {country}");
            return Results.Json(new { error = ex.Message }, statusCode: 500);
        }
    }

    public async Task<IResult> _InternalApiGetInvestorDataByCityAsync([FromQuery(Name = "city")] string city)
    {
        if (string.IsNullOrEmpty(city))
        {
            logger.LogError("No Data returned. Check the payload again.");
            return Results.BadRequest(new { error = "city is required" });
        }
        try
        {
            var data = await _repo.GetInvestorByCityFromDBAsync(city: city);
            if (data is null)
            {
                return Results.NotFound(new { error = $"Investor with '{city}' was not found." });
            }
            return Results.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving investor: {city}");
            return Results.Json(new { error = ex.Message }, statusCode: 500);
        }
    }

    // public async Task<IResult> _InternalApiGetInvestorsDataAsync([FromBody] InvestorDto investorDto)
    // {
    //     Console.WriteLine($"Fetched investorDto>>>>: {investorDto}");
    //     if (investorDto is null)
    //     {
    //         logger.LogError("No request payload given. Check the payload again.");
    //         return Results.Json(new { error = "request payload is null" });
    //     }
    //     try
    //     {
    //         var data = await _repo.GetInvestorsFromDBAsync(investorDto: investorDto);
    //         if (data is null)
    //         {
    //             return Results.Json(new { message= "No data found" }, statusCode: 200);        
    //         }
    //         Console.WriteLine($"DATA: {data}");
    //         return Results.Ok(data);
    //     }
    //     catch (Exception ex)
    //     {
    //         return Results.Json(new { error = ex.Message }, statusCode: 500);
    //     }
    // }

    // public async Task<IResult> _InternalApiGetFundsDataAsync(FundDto fundDto)
    // {
    //     if (fundDto is null)
    //     {
    //         logger.LogError("No request payload given. Check the payload again.");
    //         return Results.BadRequest(new { error = "request payload is null" });
    //     }
    //     try
    //     {
    //         var data = await _repo.GetFundsFromDBAsync(fundDto: fundDto);
    //         return Results.Ok(data);
    //     }
    //     catch (Exception ex)
    //     {
    //         return Results.Json(new { error = ex.Message }, statusCode: 500);
    //     }
    // }
}