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

        

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.FirstOrDefaultAsync(p => p.Id == pictureId);
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures.
                Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string title, string minRating, string maxRating)
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
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }

            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.TouristRoutePictures.Remove(picture);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        
    }
}
