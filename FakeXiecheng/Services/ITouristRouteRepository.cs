using FakeXiecheng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes(string title, string minRating, string maxRating);
        TouristRoute GetTouristRoute(Guid touristRouteId);
        bool TouristRouteExists(Guid touristRouteId);
        IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId);
        TouristRoutePicture GetPicture(int pictureId);
        void AddTouristRoute(TouristRoute touristRoute);
        bool Save();
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
    }
}
