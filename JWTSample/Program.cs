using JWTSample.Db;
using JWTSample.Db.Models;
using Lavshyak.AspNetCore.Identity.Extensions;
using Lavshyak.Extensions.DependencyInjection.ApplicationInitializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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

var accountAuthentication = builder.Services.AddAuthentication(options =>
{
});

accountAuthentication.AddBearerToken(IdentityConstants.ApplicationScheme, options =>
{
    //options.BearerTokenProtector = new BearerTokenNotProtector();
});

/*builder.Services.AddAuthorization(options =>
{
    /*var policy = new AuthorizationPolicy([new NameAuthorizationRequirement(IdentityConstants.BearerScheme)],
        [IdentityConstants.BearerScheme]);
    options.DefaultPolicy = policy;

    options.AddPolicy("AccountBearer", policy);#1#
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.Services.InjectServicesAndInvokeInNewScopeAsync<MainDbContext>(async mainDbContext =>
    await mainDbContext.Database.MigrateAsync()
);

app.Run();