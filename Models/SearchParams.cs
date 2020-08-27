using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crudTest.Models
{
    public class SearchParams
    {
        public string Description { get; set; }
        public int? Rank { get; set; }
        public double? Energy { get; set; }
        public double? Protein { get; set; }
        public double? Fats { get; set; }

        public List<IceCream> IceCreams;
    }
}