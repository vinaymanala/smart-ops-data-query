using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace core.contracts;

// Request
public record InvestorByIdDto(
    [Required(AllowEmptyStrings =false), RegularExpression(@"^INV-\d+$", ErrorMessage = "SearchId must match the format 'INV-001'")]
    string investorId
);

public record FundByIdDto(
    [Required(AllowEmptyStrings =false), RegularExpression(@"^FND-\d+$", ErrorMessage = "SearchId must match the format 'FND-001'")]
    string fundId
);

public record InvestorDto(
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string name,
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string type,
    [Optional,StringLength(maximumLength: 100)]
    string country,
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string region,
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string city    
);

public record FundDto(
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string name,
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string strategy,
    [Optional,StringLength(maximumLength: 100,MinimumLength = 3)]
    string exchangeCode    
);

// Response
public record InvestorResponse(
    string investor_id,
    string investor_name,
    string investor_type,
    string country,
    string region,
    string city,
    DateTime created_date
);

public record FundResponse(
    string fund_id,
    string investor_id,
    string fund_name,
    string strategy,
    string exchange_code,
    double market_price,
    long shares_held,
    double avg_market_value,
    double position_change_pct,
    double aum_usd_m,
    DateTime report_date
);