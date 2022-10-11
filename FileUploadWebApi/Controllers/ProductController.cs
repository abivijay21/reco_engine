using FileUploadWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadWebApi.Controllers
{
    [Route("api/[controller]/{muid?}")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        List<int> reg_price = new List<int>{6542,2123,3311,5231,5829,7478,2112,9181,};
        List<int> deal_price = new List<int> { 654, 213, 311, 521, 589, 478, 212, 981 };
        List<int> discount_list = new List<int> { 5, 15, 75, 45, 60, 25,55,30 };
        List<string> categories = new List<string> { "shoes" , "bags", "laptops" , "furniture" ,"ethnic_wear", "western_wear" , "books" ,"groceries" };
        List<string> retailer_list = new List<string> { "ABC" , "XYZ", "RT1" , "RT2" ,"RT3", "RT4" , "RT5" ,"RT6" };
        List<bool> coupon_list = new List<bool> {true,false,true,true,false,true,false,false };
        List<bool> cashback_list = new List<bool> { false,true,true,false,true,true,true,false };



        [HttpGet]
        public Product Get(string muid)
        {
            // Fields for individual Muid - Regular price,  Deal price, discount ,coupon(optional), cashback.
            // Metrics required - avg price,avg savings,category, total no of categories(coverage).
            // Need to frame the logic for object instantiation.
            Product product = new Product();
            product.muid = muid;

            Random rnd = new Random();
            int randIndex = rnd.Next(reg_price.Count);
            int random = reg_price[randIndex];

            product.regular_price = random;

            //randIndex = rnd.Next(reg_price.Count);
            random = deal_price[randIndex];
            product.deal_price = random;
            //     product.coupon = true;

            random = discount_list[randIndex];

            product.discount = random;

            string random_string = categories[randIndex];

            product.category = random_string;

            random_string = retailer_list[randIndex];

            product.retailers = random_string;

            bool random_b = coupon_list[randIndex];

            product.cash_back = random_b;

            random_b = cashback_list[randIndex];

            product.coupon = random_b;

            return product;
        }
    }
}
