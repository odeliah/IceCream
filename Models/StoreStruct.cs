using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crudTest.Models
{
    public class StoreStruct
    {
        public Store Store { get; set; }

        public List<IceCream> IceCreams { get; set; }

        public StoreStruct()
        {
            IceCreams = new List<IceCream>();
            Store = new Store();
        }
    }
}