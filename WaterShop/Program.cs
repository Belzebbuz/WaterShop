using System.Net;
using WaterShop;
using WaterShop.Persistance;
using WaterShop.Contracts;
using WaterShop.Middlewares;
using WaterShop.ConfigOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddConfigOptions(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddSingleton<IBalanceService, BalanceService>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddPersistance(builder.Configuration, builder.Environment.ContentRootPath);
var app = builder.Build();
await app.MigrateDbAsync();
app.UseMiddleware<ExceptionMiddleware>();
app.Use(async (ctx, next) =>
{
	if(ctx.Request.Path.HasValue && ctx.Request.Path.Value.Contains("admin.html"))
	{
		ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		return;
	}
	await next();
});
app.UseStaticFiles();
app.MapControllers();
app.MapGet("/", (HttpContext context) => context.Response.Redirect("index.html"));
app.MapGet("admin", (IWebHostEnvironment env, SecretKeyOptions options,  string key) =>
{
	if (options.SecretKey != key)
		return Results.Unauthorized();

	var admin = File.ReadAllText(Path.Combine(env.WebRootPath, "admin.html"));
	return Results.Content(admin, "text/html");
});
app.Run();