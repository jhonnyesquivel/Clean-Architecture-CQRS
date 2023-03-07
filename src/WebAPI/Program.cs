using Microsoft.IdentityModel.Tokens;
using Zemoga_Test.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebAPIServices();


//builder.Services.AddAuthentication().AddIdentityServerJwt();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

//app.MapRazorPages();

app.MapControllers();

//app.MapFallbackToFile("index.html");


app.Run();