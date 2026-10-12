using System.ComponentModel;
using System.Text.Json;
using core.contracts;
using Microsoft.SemanticKernel;

namespace core.gateway.plugins;

public class QueryInvestorFundsDataPlugin(
IHttpClientFactory httpClientFactory,
ILogger<QueryInvestorFundsDataPlugin> logger
)
{
    [KernelFunction("get_investor_data_by_id")]
    [Description("Retrieves the investor data by investor id")]
    public async Task<InvestorResponse?> GetInvestorDataByIdAsync([Description("The exact ID of the investor, e.g., INV-1001")] string investorId)
    {
        try
        {
            HttpClient client = httpClientFactory.CreateClient("coreapiclient");

            var sanitizedId = investorId.Replace("'", "").Replace("\"", "").Trim();
            var url = $"/api/internal/getInvestorDataById?searchId={sanitizedId}";

            return await client.GetFromJsonAsync<InvestorResponse>(url, new JsonSerializerOptions(JsonSerializerDefaults.Web));


        }
        catch (Exception ex)
        {
            logger.LogError("Error getting data: {Error}", ex);
            throw new Exception(ex.ToString());
        }
    }
    [KernelFunction("get_fund_data_by_id")]
    [Description("Retrieves the fund data by fund id")]
    public async Task<FundResponse?> GetFundDataByIdAsync([Description("The exact ID of the fund, e.g., FND-5001")] string fundId)
    {
        HttpClient client = httpClientFactory.CreateClient("coreapiclient");
        try
        {
            var sanitizedId = fundId.Replace("'", "").Replace("\"", "").Trim();
            var url = $"/api/internal/getFundDataById?searchId={sanitizedId}";

            return await client.GetFromJsonAsync<FundResponse>(url, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        }
        catch (Exception ex)
        {
            logger.LogError("Error getting data: {Error}", ex);
            throw new Exception(ex.ToString());
        }
    }

    [KernelFunction("get_investors_data_by_region_name")]
    [Description("Retrieves the investors data by region name. Get me investors data for region e.g., Europe")]
    public async Task<string> GetInvestorDataByRegionAsync([Description("The exact name of the region, e.g., Europe")] string region)
    {
        try
        {
            HttpClient client = httpClientFactory.CreateClient("coreapiclient");

            var sanitizedName = region.Replace("'", "").Replace("\"", "").Trim();
            var url = $"/api/internal/getInvestorDataByRegion?region={sanitizedName}";

            var response = await client.GetFromJsonAsync<IEnumerable<InvestorResponse>>(url, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return JsonSerializer.Serialize(response?.Take(20));


        }
        catch (Exception ex)
        {
            logger.LogError("Error getting data: {Error}", ex);
            throw new Exception(ex.ToString());
        }
    }

    [KernelFunction("get_investors_data_by_country_code")]
    [Description("Retrieves the investors data by country code. Get me investors data for country e.g., JP")]
    public async Task<string> GetInvestorDataByCountryAsync([Description("The exact country code of the investor, e.g., US")] string country)
    {
        try
        {
            HttpClient client = httpClientFactory.CreateClient("coreapiclient");

            var sanitizedName = country.Replace("'", "").Replace("\"", "").Trim();
            var url = $"/api/internal/getInvestorDataByCountry?country={sanitizedName}";

            var response = await client.GetFromJsonAsync<IEnumerable<InvestorResponse>>(url, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return JsonSerializer.Serialize(response?.Take(20));
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting data: {Error}", ex);
            throw;
        }
    }

    [KernelFunction("get_investors_data_by_city_name")]
    [Description("Retrieves the investors data by city name. Get me investors data for city e.g., Tokyo")]
    public async Task<string> GetInvestorDataByCityAsync([Description("The exact name of the city, e.g., Tokyo")] string city)
    {
        try
        {
            HttpClient client = httpClientFactory.CreateClient("coreapiclient");

            var sanitizedName = city.Replace("'", "").Replace("\"", "").Trim();
            var url = $"/api/internal/getInvestorDataByCity?city={sanitizedName}";

            var response = await client.GetFromJsonAsync<IEnumerable<InvestorResponse>>(url, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return JsonSerializer.Serialize(response?.Take(20));

        }
        catch (Exception ex)
        {
            logger.LogError("Error getting data: {Error}", ex);
            throw new Exception(ex.ToString());
        }
    }


    // [KernelFunction("get_investors_data_by_filter_apply")]
    // [Description("Retrieves investor data by applying specific filters mapped from the user's prompt. All filters are optional")]
    // public async Task<IResult> GetInvestorsDataAsync(InvestorDto investorDto)
    // {
    //     HttpClient client = httpClientFactory.CreateClient("coreapiclient");
    //     try
    //     {
    //         Console.WriteLine($"[PLUGIN CALLED] investor filters passed");
    //         var url = $"/api/internal/investorsData";
    //         Console.WriteLine($"[PLUGIN DETAILS] Target URL route generated: {url}");
    //         var response = await client.PostAsJsonAsync(url, investorDto);
    //         // response.EnsureSuccessStatusCode();

    //         return Results.Ok($"Message: {response?.ToString()}");

    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError("Error getting data: {Error}", ex);
    //         throw;
    //     }
    // }

    // [KernelFunction("get_funds_data_by_filter_apply")]
    // [Description("Retrieves the funds data by applying multiple filters such as name, strategy, exchangeCode or report_date")]
    // public async Task<IResult> GetFundsDataAsync(FundDto fundDto)
    // {
    //     HttpClient client = httpClientFactory.CreateClient("coreapiclient");
    //     try
    //     {
    //         var url = $"/api/internal/fundsData";
    //         var response = await client.PostAsJsonAsync(url, fundDto);

    //         response.EnsureSuccessStatusCode();

    //         return Results.Ok($"Message: {response?.ToString()}");

    //     }
    //     catch (Exception ex)
    //     {

    //         logger.LogError("Error getting data: {Error}", ex);
    //         throw;
    //     }
    // }


}