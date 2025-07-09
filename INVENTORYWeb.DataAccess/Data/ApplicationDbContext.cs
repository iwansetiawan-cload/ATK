using INVENTORYWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INVENTORYWeb.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<MSUDC> MSUDC { get; set; }
    public DbSet<CATEGORY> CATEGORY { get; set; }
    public DbSet<MASTER_PROJECT> MASTER_PROJECT { get; set; }
    public DbSet<ITEMS> ITEMS { get; set; }
    public DbSet<REQUEST_ITEM_HEADER> REQUEST_ITEM_HEADER { get; set; }
    public DbSet<REQUEST_ITEM_DETAIL> REQUEST_ITEM_DETAIL { get; set; }    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
