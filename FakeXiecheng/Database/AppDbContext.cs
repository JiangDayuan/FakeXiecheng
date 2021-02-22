using FakeXiecheng.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FakeXiecheng.Database
{
    /// <summary>
    /// 代码与数据库的连接器，在代码和数据库中引导数据的流动
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }

        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TouristRoute>().HasData(new TouristRoute()
            //{
            //    Id=Guid.NewGuid(),
            //    Title="Testtitle",
            //    Description="shiyongshuoming",
            //    OriginalPrice=0,
            //    CreateTime=DateTime.UtcNow
            //});

            // 从json中导入数据
            IList<TouristRoute> touristRoutes =  GetJsonData<TouristRoute>(@"/Database/touristRoutesMockData.json");
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            IList<TouristRoutePicture> touristPictureRoutes = GetJsonData<TouristRoutePicture>(@"/Database/touristRoutePicturesMockData.json");
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristPictureRoutes);

            base.OnModelCreating(modelBuilder);
        }

        private static IList<T> GetJsonData<T>(string jsonFilePath)
        {
            var jsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + jsonFilePath);
            IList<T> structuredData = JsonConvert.DeserializeObject<IList<T>>(jsonData);
            return structuredData;
        }
    }
}
