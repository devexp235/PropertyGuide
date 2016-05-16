using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyGuide.Models
{
    [NotMapped]
    public class PropertyViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }


        public string SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

        public IEnumerable<PropertyOfferViewModel> OfferList { get; set; }
    }    
}
