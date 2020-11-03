using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class MenuOrder
    {
        public int MenuID { get; set; }
        public int OrderID { get; set; }
        public Menu Menu { get; set; }
        public Order Order { get; set; }
    }
}