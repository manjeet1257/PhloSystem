using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhloSystemController
{
    public class ProductFilter
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
        public List<string> CommonWords { get; set; } = new List<string>();
    }

}
