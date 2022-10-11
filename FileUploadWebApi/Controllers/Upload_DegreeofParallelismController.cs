using FileUploadWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace FileUploadWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Upload_DegreeofParallelismController : ControllerBase
    {
        private static IWebHostEnvironment _webHostEnvironment;
        //private const int limit = 1000;
        Stopwatch stopwatch = new Stopwatch();


        public Upload_DegreeofParallelismController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;


        }
        [HttpPost]
        [Route("upload_with_degree")]
        public IActionResult Upload([FromForm] UploadFile obj)

        {
            //List<Product> finalProducts = new List<Product>();
            List<string> output = new List<string>();
            string[] accepted_files = { "text/csv", "application/octet-stream" };
            string ext = obj.File.ContentType;
            var file_content = obj.File.OpenReadStream;
            List<string> categories = new List<string>();
            List<string> retailers = new List<string>();
            List<int> regular_price_top_90 = new List<int>();
            List<int> deal_price_top_90 = new List<int>();
            int coupon_count = 0;
            int cashback_count = 0;






            if (ext != "text/csv")
            {
                return BadRequest("File type not supported!");
            }

            if (obj.File.Length > 0)
            {
                try
                {
                    StreamReader reader = new StreamReader(obj.File.OpenReadStream());


                    string muid_line;
                    List<string> Muid_list = new List<string>();

                    while ((muid_line = reader.ReadLine()) != null)
                    {
                        if (muid_line != "")
                            Muid_list.Add(muid_line);
                    }


                    //Monitor the session - Figure it out.
                    // Task parallelism.
                    stopwatch.Start();
                    int total_regular_price = 0, count = 0;
                    Parallel.ForEach(Muid_list,
                                     new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                                     Muid =>
                        {
                            // Processing
                            // For each MUID call an API
                            //montioring the MUID,, create sessions esp  for those not hit
                            // Faking the call for now, should be framing sample API 
                            // for eg /api/Muid/muid_id

                            var url = "https://localhost:7113/api/product/" + Muid;

                            var request = WebRequest.Create(url);
                            request.Method = "GET";

                            using var webResponse = request.GetResponse();
                            using var webStream = webResponse.GetResponseStream();

                            using var reader = new StreamReader(webStream);

                            var data = reader.ReadToEnd();

                            //JObject json = JObject.Parse(data);

                            dynamic jsondata = JObject.Parse(data);

                            total_regular_price += (int)jsondata.regular_price;
                            count += 1;

                            categories.Add(Convert.ToString(jsondata.category));
                            retailers.Add(Convert.ToString(jsondata.retailers));

                            regular_price_top_90.Add(Convert.ToInt32(jsondata.regular_price));
                            deal_price_top_90.Add(Convert.ToInt32(jsondata.deal_price));

                            if (jsondata.coupon == true)
                                coupon_count++;

                            if (jsondata.cash_back == true)
                                cashback_count++;

                            // retailers_table[jsondata.retailers] = 1;
                            output.Add(data);





                        });

                    var distinct_category_count = categories.GroupBy(x => x)
                    .Select(g => new { category_name = g.Key, Count = g.Count() })
                    .ToList();

                    var distinct_retailer_count = retailers.GroupBy(x => x)
                    .Select(g => new { retailer_name = g.Key, Count = g.Count() })
                    .ToList();

                    int percent = (9 * (count)) / 10;


                    regular_price_top_90.Sort();
                    regular_price_top_90 = regular_price_top_90.GetRange(0, percent);
                    var avg_regular_price = (regular_price_top_90.AsQueryable().Sum())/percent;

                    deal_price_top_90.Sort();
                    deal_price_top_90 = deal_price_top_90.GetRange(0, percent);
                    var avg_deal_price = (deal_price_top_90.AsQueryable().Sum()) / percent;


                    var elapsedTime = stopwatch.ElapsedMilliseconds;

                    return Ok(new { output, total_regular_price, 
                        average_price = total_regular_price / count, 
                        count, distinct_category_count, distinct_retailer_count,
                      //  regular_price_top_90, deal_price_top_90, 
                        avg_regular_price,avg_deal_price,
                        cashback_count,coupon_count,
                        Total_Muids = Muid_list.Count,
                        Misssed_Muids = (Muid_list.Count - count) });



                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Upload Failed");
            }
        }
    }

}
