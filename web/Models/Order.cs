
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace web.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public ApplicationUser? Client { get; set; }

        public string Comment { get; set; }

        public ICollection<MenuOrder> MenuOrders { get; set; }

    }
}