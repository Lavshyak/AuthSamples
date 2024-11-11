using JWTSandbox.Db.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTSandbox.Db;

public class MainDbContext : IdentityDbContext<Account, AccountRole, long>
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }
}