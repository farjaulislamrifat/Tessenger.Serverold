using Microsoft.AspNetCore.Mvc;
using Tessenger.Server.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tessenger.Server.Data;
using Microsoft.OpenApi.Models;
using Tessenger.Server.Hubs;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<TessengerServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TessengerServerContext") ?? throw new InvalidOperationException("Connection string 'TessengerServerContext' not found.")) , ServiceLifetime.Transient );
                    
builder.Services.AddDbContext<TessengerServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TessengerServerContext") ?? throw new InvalidOperationException("Connection string 'TessengerServerContext' not found.")) , ServiceLifetime.Transient );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddScoped<Hub_Methods>();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme  ()
    {

        Description = "Api Key To Secure Api",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "api-key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "ApiKeyScheme"


    });


    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey",
        }
        ,
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
     {
         {scheme ,new List<string>() }
     };

    x.AddSecurityRequirement(requirement);
});

builder.Services.AddControllers();
builder.Services.AddScoped<AuthServiceFillter>();
builder.Services.AddScoped<AuthServiceFillterForTemp>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<Hub_Methods>("/Hub");
app.MapGet("/", () =>
{
    var data = """
    Welcome To Tessenger Api. 
    {
       "AppVersion" : "1.0.0"
    }
    """;
          return data ;
});


app.Run();


