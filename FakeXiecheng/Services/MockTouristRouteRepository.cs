using FakeXiecheng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Services
{
    public class MockTouristRouteRepository : ITouristRouteRepository
    {
        private List<TouristRoute> _routes;

        public MockTouristRouteRepository()
        {
            if (_routes == null)
            {
                InitializeTouristRoutes();
            }
        }

        private void InitializeTouristRoutes()
        {
            _routes = new List<TouristRoute>
            {
                new TouristRoute
                {
                    Id = Guid.NewGuid(),
                    Title = "黄山",
                    Description = "黄山真好玩",
                    OriginalPrice = 1299,
                    Features = "<p>吃住行旅游购物娱乐</p>",
                    Fees = "<p>交通费用自理</p>",
                    Notes = "<p>小心危险</p>"
                },
                new TouristRoute
                {
                    Id = Guid.NewGuid(),
                    Title = "华山",
                    Description = "华山真好玩",
                    OriginalPrice = 1399,
                    Features = "<p>吃住行旅游购物娱乐</p>",
                    Fees = "<p>交通费用自理</p>",
                    Notes = "<p>小心危险</p>"
                },
            };
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            // linq
            return _routes.FirstOrDefault(n => n.Id == touristRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes()
        {
            return _routes;
        }
    }
}
