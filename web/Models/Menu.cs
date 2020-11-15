
using System;
using System.Collections.Generic;

namespace web.Models
{

    public class Menu
    {
        public int MenuID { get; set; }
        public string FoodName { get; set; }
        public string FoodType { get; set; } // main, soup, dessert 
        public string MenuType { get; set; } //  meat, vegan, no sugar, no alergens

        public ICollection<MenuOrder> MenuOrders { get; set; }

        public ICollection<Rating> Ratings { get; set; }


    }
}