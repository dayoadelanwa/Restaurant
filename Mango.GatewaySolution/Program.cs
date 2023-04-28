using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
        builder.Services.AddOcelot();
    });


app.MapGet("/", () => "Hello World!");

app.UseOcelot();
app.Run();
