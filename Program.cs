using CryptidCare.Data;
using CryptidCare.Data.Repositories;
using CryptidCare.Services;
using CryptidCare.Services.Rules;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CryptidCareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddScoped<IClaimRule, SilverAllergyRule>();
builder.Services.AddScoped<IClaimRule, HydraHeadMultiplierRule>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
