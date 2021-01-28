using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }

        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }


    }
}
