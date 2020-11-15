
using System;
using System.ComponentModel.DataAnnotations;


namespace web.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public ApplicationUser? Client { get; set; }
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        [Range(1, 5)]

        public int value { get; set; }

    }
}