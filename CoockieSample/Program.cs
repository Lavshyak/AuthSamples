using JWTSandbox.Db;
using JWTSandbox.Db.Models;
using Lavshyak.AspNetCore.Identity.Extensions;
using Lavshyak.Extensions.DependencyInjection.ApplicationInitializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("maindb"),
        sqlOptions => { sqlOptions.MigrationsAssembly(typeof(MainDbContext).Assembly.GetName().FullName); });
});

builder.Services.TryAddIdentityDeps();

builder.Services.AddIdentityNormal<Account, AccountRole>()
    .AddSignInManagerNormal<SignInManager<Account>, Account, long>()
    .AddEntityFrameworkStores<MainDbContext>();

builder.Services.AddAuthentication().AddApplicationCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

await app.Services.InjectServicesAndInvokeInNewScopeAsync<MainDbContext>(async mainDbContext =>
    await mainDbContext.Database.MigrateAsync()
);

app.Run();