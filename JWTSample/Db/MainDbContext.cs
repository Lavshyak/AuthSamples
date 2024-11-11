using JWTSample.Db.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTSample.Db;

public class MainDbContext : IdentityDbContext<Account, AccountRole, long>
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }
}