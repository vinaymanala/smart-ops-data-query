using core.coreapi.endpoints;
using core.coreapi.svc;
using core.data;
using core.data.repositories;
using core.extensions;
using core.gateway.endpoints;
using core.gateway.plugins;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddCors(options =>
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://myfrontend.com") // Allowed domains
              .AllowAnyHeader()
              .AllowAnyMethod();
    }));

    builder.Services.AddHttpClient("coreapiclient", client =>
    {
        client.BaseAddress = new Uri(!String.IsNullOrEmpty(builder.Configuration.GetValue<string>("ASPNETCORE_URLS")) ? "http://localhost:8080" : "http://localhost:5047");
        // client.DefaultRequestHeaders.Add("Authorization", "Bearer your-token");
        client.Timeout = TimeSpan.FromSeconds(30);
    });

    builder.Services.AddSemanticKernelService(builder.Configuration);

    // Register Dapper Context
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddSingleton<IDbConnectionFactory>(new SqlDbConnectionFactory(connectionString));

    builder.Services.AddTransient<QueryInvestorFundsDataPlugin>();
    builder.Services.AddTransient<RagServicePlugin>();

    builder.Services.AddTransient<IInvestosFundsRepository, InvestosFundsRepository>();
    builder.Services.AddTransient<IInvestorsFundsSvc, InvestorsFundsSvc>();
    builder.Services.AddTransient<InvestorsFundsSvc>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
var kernel = app.Services.GetRequiredService<Kernel>();

var investorsFundsSvc = app.Services.GetRequiredService<InvestorsFundsSvc>();

app.MapChatEndpoints(kernel);
app.MapSearchEndpoints(kernel);

app.MapInvestorsFundsEndpoints(investorsFundsSvc);
app.Run();