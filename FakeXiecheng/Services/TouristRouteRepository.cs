using FakeXiecheng.Database;
using FakeXiecheng.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeXiecheng.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
            
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return _context.TouristRoutePictures.FirstOrDefault(p => p.Id == pictureId);
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(n => n.Id == touristRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId)
        {
            return _context.TouristRoutePictures.
                Where(p => p.TouristRouteId == touristRouteId).ToList();
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(string title, string minRating, string maxRating)
        {
            // 生产SQL语句，还没有查询(延迟执行)
            IQueryable<TouristRoute> result = _context.TouristRoutes.
                Include(t => t.TouristRoutePictures);

            // Title模糊查询
            if (!String.IsNullOrWhiteSpace(title))
            {
                title = title.Trim();
                result = result.Where(t => t.Title.Contains(title));
            }

            // Rating范围查询
            Regex regexInt = new Regex(@"\d+");
            Match matchMinRating = regexInt.Match(minRating);
            Match matchMaxRating = regexInt.Match(maxRating);
            if (matchMinRating.Success)
            {
                result = result.Where(t => t.Rating >= Int32.Parse(minRating));
            }
            if (matchMaxRating.Success)
            {
                result = result.Where(t => t.Rating <= Int32.Parse(maxRating));
            }
            //if (ratingValue >= 0)
            //{
            //    switch (operatorType)
            //    {
            //        case "largerThan": result = result.Where(t => t.Rating >= ratingValue);
            //            break;
            //        case "lessThan":
            //            result = result.Where(t => t.Rating <= ratingValue);
            //            break;
            //    }
            //}
            // Include 通过外键将两张表链接在一起
            // Join 通过手动的方式链接表格

            // 聚合操作，执行sql语句 ToList(), SingleOrDefault(), Count()
            return result.ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }
    }
}
