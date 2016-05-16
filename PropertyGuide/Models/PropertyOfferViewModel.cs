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
    [NotMapped]
    public class PropertyOfferViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string BuyerId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [DisplayName("Price Offered")]
        [Range(1000, 100000)]
        [DataType(DataType.Currency)]
        public decimal PriceOffered { get; set; }

        public bool OfferSubmitted { get; set; }

        public DateTime? DateAccepted { get; set; }
        public DateTime? DateRejected { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime PropertyDateCreated { get; set; }

        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }
        public string BuyerEmail { get; set; }

        public string SellerFirstName { get; set; }
        public string SellerLastName { get; set; }

        public string BuyerFullName
        {
            get { return string.Format("{0} {1}", BuyerFirstName, BuyerLastName); }
        }

        public string SellerFullName
        {
            get { return string.Format("{0} {1}", SellerFirstName, SellerLastName); }
        }

        public bool IsAccepted
        {
            get
            {
                return DateAccepted.HasValue;
            }
        }

        public bool IsRejected
        {
            get
            {
                return DateRejected.HasValue;
            }
        }
    }
}
