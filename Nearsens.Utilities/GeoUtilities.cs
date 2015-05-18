using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearsens.Utilities
{
    public class GeoUtilities
    {
        private static double R = 6371;
        public static double CalculateDistance(double lat1, double lat2, double lng1, double lng2)
        {
            var lat_alfa = Math.PI * lat1 / 180;
            var lat_beta = Math.PI * lat2 / 180;
            var lng_alfa = Math.PI * lng1 / 180;
            var lng_beta = Math.PI * lng2 / 180;

            var fi = Math.Abs(lng_alfa - lng_beta);

            var p = Math.Acos(Math.Sin(lat_beta) * Math.Sin(lat_alfa) + Math.Cos(lat_beta) * Math.Cos(lat_alfa) * Math.Cos(fi));
            var distance = p * R;
            
            return distance;
        }
    }
}
