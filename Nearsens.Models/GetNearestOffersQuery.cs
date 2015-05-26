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
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string MainPhoto { get; set; }
        public string PlaceName { get; set; }
        public double PlaceLat { get; set; }
        public double PlaceLng { get; set; }
    }
}
