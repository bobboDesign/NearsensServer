using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearsens.Models
{
    public class GetOfferQuery
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public double Price { get; set; }
        public double PreviousPrice { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public IEnumerable<string> Photos { get; set; }
    }
}
