using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models.ViewModels
{
    public class OrderedMenuData
    {
        public int MenuID { get; set; }
        public string FoodName { get; set; }
        public bool Ordered { get; set; }
    }
}