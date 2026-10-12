using System.Dynamic;
using core.contracts;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace core.data.repositories;

public interface IInvestosFundsRepository
{
    Task<Investor?> GetInvestorByIdFromDBAsync(string searchId);
    Task<IEnumerable<Investor>> GetInvestorByRegionFromDBAsync(string region);
    Task<IEnumerable<Investor>> GetInvestorByCountryFromDBAsync(string country);
    Task<IEnumerable<Investor>> GetInvestorByCityFromDBAsync(string city);
    Task<Fund> GetFundByIdFromDBAsync(string searchId);
    // Task<IEnumerable<Investor>?> GetInvestorsFromDBAsync(InvestorDto investorDto);
    // Task<IEnumerable<Fund>> GetFundsFromDBAsync(FundDto fundDto);
}
public class InvestosFundsRepository(IDbConnectionFactory connectionFactory) : IInvestosFundsRepository
{
    public async Task<Investor?> GetInvestorByIdFromDBAsync(string searchId)
    {
        using var conn = connectionFactory.CreateConnection();
        var sp = "sp_GetInvestorById";        
        return await conn.QuerySingleAsync<Investor>(sp, new { InvestorId = searchId }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Fund> GetFundByIdFromDBAsync(string searchId)
    {
        using var conn = connectionFactory.CreateConnection();
        var sp = "sp_GetFundById";
        return await conn.QuerySingleAsync<Fund>(sp, new { FundId = searchId }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Investor>> GetInvestorByRegionFromDBAsync(string region)
    {
        using var conn = connectionFactory.CreateConnection();
        var sp = "sp_GetInvestorsByRegionName";
        return await conn.QueryAsync<Investor>(sp, new { Region = region }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Investor>> GetInvestorByCountryFromDBAsync(string country)
    {
        using var conn = connectionFactory.CreateConnection();
        var sp = "sp_GetInvestorsByCountryCode";
        return await conn.QueryAsync<Investor>(sp, new { Country = country }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Investor>> GetInvestorByCityFromDBAsync(string city)
    {
        using var conn = connectionFactory.CreateConnection();
        var sp = "sp_GetInvestorsByCityName";
        return await conn.QueryAsync<Investor>(sp, new { City = city }, commandType: System.Data.CommandType.StoredProcedure);
    }
    

    // public async Task<IEnumerable<Investor>?> GetInvestorsFromDBAsync(InvestorDto investorDto)
    // {
    //     using var conn = connectionFactory.CreateConnection();
    //     var sp = "sp_GetInvestors";

    //     if (investorDto is null)
    //     {
    //         return Array.Empty<Investor>();
    //     }
    //     string SanitizeParamter(string? parameter) => parameter is not null
    //     ? parameter.Replace("'", "").Replace("\"", "").Trim()
    //     : String.Empty;

    //     var payload = new Dictionary<string, string>();
    //     void AddIfNotEmpty(string key, string? rawValue)
    //     {
    //         string sanitized = SanitizeParamter(rawValue);
    //         if (!string.IsNullOrEmpty(sanitized))
    //         {
    //             payload.Add(key, sanitized);
    //         }
    //     }
    //     AddIfNotEmpty("Name", investorDto?.name);
    //     AddIfNotEmpty("Type", investorDto?.type);
    //     AddIfNotEmpty("Country", investorDto?.country);
    //     AddIfNotEmpty("Region", investorDto?.region);
    //     AddIfNotEmpty("City", investorDto?.city);
    //     var json = JsonConvert.SerializeObject(payload);
    //     var dto = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
    //     Console.WriteLine($"Sending parameters to Dapper: {dto}");
    //     try
    //     {
    //         return await conn.QueryAsync<Investor>(sp, new {Country = "US"}, commandType: System.Data.CommandType.StoredProcedure);
    //     }
    //     catch (Exception ex)
    //     {

    //         Console.WriteLine(ex);
    //         return null;
    //     }
    // }

    // public async Task<IEnumerable<Fund>> GetFundsFromDBAsync(FundDto fundDto)
    // {
    //     using var conn = connectionFactory.CreateConnection();
    //     var sp = "sp_GetFunds";
    //     return await conn.QueryAsync<Fund>(sp, fundDto, commandType: System.Data.CommandType.StoredProcedure);
    // }

}