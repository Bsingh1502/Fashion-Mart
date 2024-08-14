using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FashionMart.Models
{
    public class Checkout
    {
        [Required, Display(Name = "Name")]
        public string ShippingName { get; set; }

        [Required, Display(Name = "Address")]
        public string ShippingAddress { get; set; }

        [Required, Display(Name = "Country")]
        public string ShippingCountry { get; set; }

        [Required, Display(Name = "Card Number"), CreditCard]
        public string CardNumber { get; set; }

        [Required, Display(Name = "Expiry Date"), RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Invalid expiry date")]
        public string CardExpiry { get; set; }

        [Required, Display(Name = "CVV"), RegularExpression(@"^\d{3,4}$", ErrorMessage = "Invalid CVV")]
        public string CardCVV { get; set; }
    }
}
