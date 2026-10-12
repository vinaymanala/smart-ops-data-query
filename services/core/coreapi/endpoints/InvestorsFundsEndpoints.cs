
using core.coreapi.svc;

namespace core.coreapi.endpoints;

public static class InvestorsFundsEndpoints
{
    public static void MapInvestorsFundsEndpoints(this WebApplication application, InvestorsFundsSvc svc)
    {        
        var group = application.MapGroup("/api/internal").WithTags("Internal APIs");

        group.MapGet("/getInvestorDataById", svc._InternalApiGetInvestorDataByIdAsync);
        group.MapGet("/getFundDataById", svc._InternalApiGetFundDataByIdAsync);
        group.MapGet("/getInvestorDataByRegion", svc._InternalApiGetInvestorDataByRegionAsync);
        group.MapGet("/getInvestorDataByCountry", svc._InternalApiGetInvestorDataByCountryAsync);
        group.MapGet("/getInvestorDataByCity", svc._InternalApiGetInvestorDataByCityAsync);
        // group.MapPost("/investorsData", svc._InternalApiGetInvestorsDataAsync);
        // group.MapPost("/fundsData", svc._InternalApiGetFundsDataAsync);
    }

    
}