using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models.ViewModels
{
    public class OrderIndexData
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Menu> Menus { get; set; }
    }
}