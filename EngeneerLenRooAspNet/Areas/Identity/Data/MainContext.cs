using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngeneerLenRooAspNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EngeneerLenRooAspNet.Areas.Identity.Data
{
    public class MainContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Technique> Techniques { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Cartridge> Cartridges { get; set; }
        
        
        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
        }
    }
}
