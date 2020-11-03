
using System;
using System.Collections.Generic;

namespace web.Models
{

    // public enum MenuType
    // {
    //     Mesni, Veganski, brezSkladkorja, brezAlergenov
    // }

    // public enum FoodType
    // {
    //     Soup, Noodles, Meat, Dessert
    // }

    public class Menu
    {
        public int MenuID { get; set; }
        // public MenuType? MenuType { get; set; }

        // public FoodType? FoodType { get; set; }
        public string FoodType { get; set; }

        public string FoodName { get; set; }

        public ICollection<MenuOrder> MenuOrders { get; set; }


    }
}