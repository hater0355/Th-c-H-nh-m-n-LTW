using Thực_Hành_Web.Services;
using Microsoft.EntityFrameworkCore;
using Thực_Hành_Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(name => $"Trường {name} phải là số.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(value => $"Giá trị '{value}' không hợp lệ.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, name) => $"Giá trị '{value}' không hợp lệ cho trường {name}.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(name => $"Vui lòng nhập {name}.");
});
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
builder.Services.AddSingleton<StudentStore>();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseMigrationsEndPoint();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await DbInitializer.InitializeAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
