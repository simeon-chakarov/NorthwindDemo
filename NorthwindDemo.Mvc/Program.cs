using NorthwindDemo.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(client =>
{
    var baseUrl = builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7013";
    client.BaseAddress = new Uri(baseUrl);
    //client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>((sp, client) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["Api:BaseUrl"] ?? "https://localhost:7013";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customers}/{action=Index}/{id?}");

app.Run();
