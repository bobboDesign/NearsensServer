using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearsens.Models
{
    public class GetPlacesByUserIdQuery
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
