using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using INVENTORYWeb.DataAccess.Data;
using INVENTORYWeb.Models;
using INVENTORYWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(OI.Role_Admin).GetAwaiter().GetResult())
            {              
                _roleManager.CreateAsync(new IdentityRole(OI.Role_SuperAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(OI.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(OI.Role_User)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Administrator",
                    Email = "iwan.setti@gmail.com",
                    Name = "Administrator",
                    FullName = "Administrator",
                    Gender = "Laki Laki",
                    PhoneNumber = "0821",
                    Divisi = "IT",
                    Role = "SuperAdmin",
                    RolesName = "SuperAdmin",
                    Flag = 1,
                }, "P@ssw0rd123").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "iwan.setti@gmail.com");

                _userManager.AddToRoleAsync(user, OI.Role_SuperAdmin).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
