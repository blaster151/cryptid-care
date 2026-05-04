using CryptidCare.Services;
using CryptidCare.Services.Rules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddScoped<IClaimRule, SilverAllergyRule>();
builder.Services.AddScoped<IClaimService, ClaimService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
