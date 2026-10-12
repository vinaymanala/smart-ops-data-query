namespace core.data;

public record Investor
(
    string investor_id,
    string investor_name,
    string investor_type,
    string country,
    string region,
    string city,
    DateTime created_date
);

public record Fund(
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
