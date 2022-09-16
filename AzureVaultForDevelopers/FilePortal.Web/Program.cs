using Azure.Identity;
using FilePortal.Dal;
using FilePortal.Dal.Model;
using FilePortal.FileService.Config;
using FilePortal.SecureVault.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var vaultConfig = builder.Configuration.GetSection("KeyVault").Get<VaultConfiguration>();

builder.Configuration.AddAzureKeyVault(new Uri(vaultConfig.Endpoint),new DefaultAzureCredential());

// Add DB
var dbConenctionString = builder.Configuration.GetConnectionString("PotalAppDBConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>  options.UseSqlServer(dbConenctionString));

//Add Storage
var storageConfig = builder.Configuration.GetSection("StorageConfig").Get<StorageConfig>();
builder.Services.RegisterFileServices(storageConfig);
builder.Services.RegisterVaultServices(vaultConfig);


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//configure authentication
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");

}

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCertificateForwarding();

app.UseAuthentication();
app.UseAuthorization();

app.SeedUsers();

app.MapRazorPages();

app.Run();
