import urllib.parse
import random
from datetime import date, timedelta
from faker import Faker
import pandas as pd
from sqlalchemy import create_engine, text

# Initialize Faker with global locales for realistic regional data
fake = Faker(["en_US", "en_GB", "en_CA", "de_DE", "ja_JP"])
Faker.seed(42)
random.seed(42)

# Global reference data for realistic financial modeling
EXCHANGES_BY_REGION = {
    "North America": [
        ("NYSE", "US"),
        ("NASDAQ", "US"),
        ("TSX", "CA"),
    ],
    "Europe": [
        ("LSE", "GB"),
        ("XETR", "DE"),
        ("SIX", "CH"),
        ("EURONEXT", "FR"),
    ],
    "Asia-Pacific": [
        ("TSE", "JP"),
        ("HKEX", "HK"),
        ("SGX", "SG"),
        ("ASX", "AU"),
    ],
}

INVESTOR_TYPES = ["Hedge Fund", "Family Office", "Pension Fund", "Sovereign Wealth", "Venture Capital", "PE Firm"]
FUND_STRATEGIES = ["Long/Short Equity", "Global Macro", "Fixed Income", "Multi-Strategy", "Distressed Debt", "Growth Equity"]


def generate_financial_dataset(record_count:int=500) -> tuple[pd.DataFrame, pd.DataFrame]:
    investors = []
    funds = []

    for i in range(1, record_count + 1):
        # 1. Geographic & Location attributes
        region = random.choice(list(EXCHANGES_BY_REGION.keys()))
        exchange_code, country = random.choice(EXCHANGES_BY_REGION[region])
        city = fake.city()

        # 2. Investor Entity
        investor_id = f"INV-{1000 + i}"
        
        
        investor_name = f"{fake.company()} {random.choice(['Capital', 'Management', 'Partners', 'Asset Management', 'Group'])}"
        investor_type = random.choice(INVESTOR_TYPES)

        investors.append({
            "investor_id": investor_id,
            "investor_name": investor_name,
            "investor_type": investor_type,
            "country": country,
            "region": region,
            "city": city,
            "created_date": fake.date_between(start_date="-5y", end_date="-1y"),
        })

        # 3. Fund Entity & Financial Metrics
        fund_id = f"FND-{5000 + i}"
        fund_name = f"{investor_name.split()[0]} {random.choice(FUND_STRATEGIES)} Fund"

        # Pricing & Position Metrics
        market_price = round(random.uniform(10.0, 1500.0), 2)
        shares_held = random.randint(1_000, 500_000)
        avg_market_value = round(market_price * shares_held, 2)

        # Position change percentage (-25.00% to +25.00%)
        position_change_pct = round(random.uniform(-0.25, 0.25), 4)

        # Report Date (13F or quarterly reporting alignment)
        report_date = fake.date_between(start_date="-1y", end_date="today")

        # Additional Core Metrics
        aum_millions = round(random.uniform(50.0, 10000.0), 2)
        expense_ratio = round(random.uniform(0.0015, 0.0250), 4)  # 0.15% to 2.50%
        sharpe_ratio = round(random.uniform(-0.5, 3.2), 2)

        funds.append({
            "fund_id": fund_id,
            "investor_id": investor_id,  # Association / Foreign Key
            "fund_name": fund_name,
            "strategy": random.choice(FUND_STRATEGIES),
            "exchange_code": exchange_code,
            "market_price": market_price,
            "shares_held": shares_held,
            "avg_market_value": avg_market_value,
            "position_change_pct": position_change_pct,
            "report_date": report_date,
            "aum_usd_m": aum_millions,
            "expense_ratio": expense_ratio,
            "sharpe_ratio": sharpe_ratio,
        })

    # Convert to pandas DataFrames
    df_investors = pd.DataFrame(investors)
    df_funds = pd.DataFrame(funds)

    return df_investors, df_funds


def mssql_connection():
    # 1. Connection Parameters
    SERVER = '127.0.0.1'       # Maps to localhost
    PORT = 1433                # Update if you changed the host port mapping (e.g., 1401)
    DATABASE = 'master'        # Default system database
    USERNAME = 'sa'
    PASSWORD = 'YourStrong_Passw0rd!'
    DRIVER = 'ODBC Driver 18 for SQL Server' # Match your installed host driver version

    # 2. Format & Encode
    safe_password = urllib.parse.quote_plus(PASSWORD)
    # TrustServerCertificate=yes is required for SQL Server 2022+ due to enforced encryption
    connection_string = (
        f"mssql+pyodbc://{USERNAME}:{safe_password}@{SERVER}:{PORT}/{DATABASE}"
        f"?driver={DRIVER}&TrustServerCertificate=yes"
    )

    # 3. Create Engine & Test
    engine = create_engine(connection_string)

    # with engine.connect() as conn:
    #     print("Successfully connected to Podman SQL Server!")
    #     print(conn.execute(text("SELECT @@VERSION")).scalar())
    print("Exporting data to MSSQL Server..")
    df_investors.to_sql("investors", engine, if_exists="replace", index=False)
    df_funds.to_sql("funds", engine, if_exists="replace", index=False)

# Execute generator
df_investors, df_funds = generate_financial_dataset(500)

# Export options
# df_investors.to_csv("investors_seed.csv", index=False)
# df_funds.to_csv("funds_seed.csv", index=False)
print(f"Generated {len(df_investors)} Investor records and {len(df_funds)} Fund records successfully.")
print("Connecting to MSSQL Server..")
mssql_connection()
print("Export completed successfully.")

