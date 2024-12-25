using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeaterApp.Models;

namespace WeaterApp.Data
{
    public class WeaterAppContext : DbContext
    {
        public WeaterAppContext (DbContextOptions<WeaterAppContext> options)
            : base(options)
        {
        }

        public DbSet<WeaterApp.Models.Weather> Weather { get; set; } = default!;
    }
}
