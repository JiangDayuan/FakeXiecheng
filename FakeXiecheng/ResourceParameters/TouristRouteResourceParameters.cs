using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.ResourceParameters
{
    public class TouristRouteResourceParameters
    {
        public string title { get; set; }

        public string minRating { get; set; } = "";
        public string maxRating { get; set; } = "";

        private string _rating { get; set; }
        public string rating {
            get { return _rating; }
            set 
            {
                //minRating = "";
                //maxRating = "";
                if (value.Contains(':'))
                {
                    minRating = value.Split(':')[0];
                    maxRating = value.Split(':')[1];
                }
                _rating = value;
            }
        }


    }
}
