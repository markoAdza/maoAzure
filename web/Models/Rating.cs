
using System.Collections.Generic;

namespace web.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public ApplicationUser? Client { get; set; }
        public int MenuID { get; set; }
        public Menu Menu { get; set; }
        public int value { get; set; }

    }
}