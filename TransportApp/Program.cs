using TransportApp;
using TransportApp.Application.Helper;
using FluentValidation;
using Wolverine;
using TransportApp.Domain.DTO;
using TransportApp.Domain.DTO.Config;
using TransportApp.Features.User;
using TransportApp.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddOpenApi();
builder.Services.AddScrutor();
builder.Services.AddDatabase(builder.Configuration);
builder.Host.UseWolverine(opts =>
{
    opts.Services.AddValidatorsFromAssemblyContaining<Program>();
    opts.Discovery.IncludeAssembly(typeof(CreateUserHandler).Assembly);
});
builder.Services.AddCorsPolicy();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
//app.UseAntiforgery();


// Run seeding when app starts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles.Add(services);
}

var endpointDefinitions = typeof(Program).Assembly
    .GetTypes()
    .Where(t => typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsInterface)
    .Select(Activator.CreateInstance)
    .Cast<IEndpointDefinition>();

foreach (var endpoint in endpointDefinitions)
{
    endpoint.MapEndpoints(app);
}

app.Run();