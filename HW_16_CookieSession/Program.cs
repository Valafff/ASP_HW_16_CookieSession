
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(1000);
//});
var app = builder.Build();
app.UseSession();

app.Map("/", () => "Main Page \n session - session mode \n cookie - cookie mode");

app.Map("/session", async (context) =>
{
    if (context.Session.Keys.Contains("testsession"))
        await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}");
    else
    {
        context.Session.SetString("testsession", "Test session");
        await context.Response.WriteAsync("Hello Session");
    }

});

app.Map("/cookie", async (context) =>
{


    if (context.Request.Cookies.ContainsKey("testcookie"))
    {
        string? name = context.Request.Cookies["testcookie"];
        await context.Response.WriteAsync($"Hello {name}");
    }
    else
    {
        context.Response.Cookies.Append("testcookie", "TestCookie");
        await context.Response.WriteAsync("Hello Cookie");
    }
});

app.Run();