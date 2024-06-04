using Dynamsoft.Core;
using Dynamsoft.License;
using Microsoft.OpenApi.Models;

string errorMsg;
int errorCode = LicenseManager.InitLicense("DLS2eyJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSJ9", out errorMsg);
if (errorCode != (int)EnumErrorCode.EC_OK)
{
    Console.WriteLine("License initialization error: " + errorMsg);
}
else {
    Console.WriteLine("License valid.");
}
   
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document Scanner API", Description = "Scan documents from images", Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Scanner API V1");
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable PNA preflight requests
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Method.Equals("options", StringComparison.InvariantCultureIgnoreCase) && ctx.Request.Headers.ContainsKey("Access-Control-Request-Private-Network"))
    {
        ctx.Response.Headers.Add("Access-Control-Allow-Private-Network", "true");
    }

    await next();
});
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
