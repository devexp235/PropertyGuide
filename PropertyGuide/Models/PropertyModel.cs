using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyGuide.Models
{    
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateSold { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [Required]
        public string SellerId { get; set; }

        [ForeignKey("SellerId")]
        public ApplicationUser Seller { get; set; }

        public ICollection<PropertyOffer> PropertyOffers { get; set; }
    }

    public class PropertyOffer
    {
        public int Id { get; set; }
        public decimal PriceOffered { get; set; }

        public DateTime? DateAccepted { get; set; }
        public DateTime? DateRejected { get; set; }

        public DateTime DateCreated { get; set; }

        public int PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public Property Property { get; set; }

        //[Required]
        public string BuyerId { get; set; }

        [ForeignKey("BuyerId")]
        public ApplicationUser Buyer { get; set; }
    }    
}
