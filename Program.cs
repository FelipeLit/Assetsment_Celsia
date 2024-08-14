using assetsment_Celsia.Data;
using assetsment_Celsia.Interfaces;
using assetsment_Celsia.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<CelsiaContext>(
    option => option.UseMySql(
    builder.Configuration.GetConnectionString("MySql"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.2")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IExcelRepository, ExcelRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IInvoicesRepository, InvoiceRepository>();



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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Signin}/{id?}");

app.Run();
