using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearsens.Models
{
    public class GetNearestOffersQuery
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Icon { get; set; }
        public string PlaceName { get; set; }
        public double PlaceLat { get; set; }
        public double PlaceLng { get; set; }
    }
}
