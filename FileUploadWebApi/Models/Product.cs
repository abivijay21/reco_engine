using Microsoft.Extensions.Options;

namespace FileUploadWebApi.Models
{
    public class Product
    {
        // Figure out the right template for mock object 
       public string muid { get; set; }
        public float regular_price { get; set; }
        public float deal_price { get; set; }
        public float discount { get; set; }
        public bool coupon { get; set; }

        public bool cash_back { get; set; }
        public string retailers { get; set; }
        public string category { get; set; }

    }
}
