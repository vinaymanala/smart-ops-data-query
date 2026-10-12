using Azure.Core.Pipeline;
using core.coreapi.endpoints;
using core.coreapi.svc;
using core.data;
using core.data.repositories;
using core.extensions;
using core.gateway.Endpoints;
using core.gateway.plugins;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddHttpClient("coreapiclient", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5047");
        // client.DefaultRequestHeaders.Add("Authorization", "Bearer your-token");
        client.Timeout = TimeSpan.FromSeconds(30);          
    });
    
    // Register Dapper Context
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddSingleton<IDbConnectionFactory>(new SqlDbConnectionFactory(connectionString));
    
    builder.Services.AddTransient<QueryInvestorFundsDataPlugin>();

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

var kernel = app.UseSemanticKernelService();

var investorsFundsSvc = app.Services.GetRequiredService<InvestorsFundsSvc>();

app.MapInvestorsFundsEndpoints(investorsFundsSvc);
app.MapChatEndpoints(kernel);
app.Run();