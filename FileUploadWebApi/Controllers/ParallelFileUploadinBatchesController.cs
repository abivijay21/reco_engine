//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Diagnostics;
//using System.Net;
//using System.Text;
//using Newtonsoft.Json;
//using FileUploadWebApi.Models;
//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using System.Collections;
//using Newtonsoft.Json.Linq;

//namespace FileUploadWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ParallelFileUploadinBatchesController : ControllerBase
//    {

//        private static IWebHostEnvironment _webHostEnvironment;

//        public ParallelFileUploadinBatchesController(IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;


//        }
//        Hashtable category_table = new Hashtable();
//        Hashtable retailers_table = new Hashtable();


//        //public MuidController()
//        //{
//        //    client = new HttpClient();
//        //}

//        [HttpGet]
//        [Route("GetId")]
//        public async Task<string> GetProduct(string Muid)
//        {
//            var url = "https://localhost:7113/api/product/" + Muid;

//            //var response = await client.GetAsync(
//            //    url)
//            //    .ConfigureAwait(false);

//            var request = WebRequest.Create(url);
//            request.Method = "GET";

//            using var webResponse = request.GetResponse();
//            using var webStream = webResponse.GetResponseStream();

//            using var reader = new StreamReader(webStream);
//            //var user = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

//           var data = reader.ReadToEnd();
//            dynamic jsondata = JObject.Parse(data);

//            if (category_table.ContainsKey(jsondata.category))
//                category_table[jsondata.category] = category_table[jsondata.category] + 1;
//            else
//                category_table[jsondata.category] = 1;
//            return data;
//           // return user;
//        }

//        //[HttpPost]
//        //[Route("getMany")]
//        //[Route("getMany")]
//        //public async Task<IEnumerable<string>> GetProductDetails(IEnumerable<string> ids)
//        //{

//        //    var response = await client
//        //        .PostAsync(
//        //            "https://localhost:7113/api/product/GetMany",
//        //            new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"))
//        //        .ConfigureAwait(false);

//        //    var users = JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());

//        //    return users;
//        //}
//        [HttpGet]
//        [Route("many_muids")]
//        public async Task<IEnumerable<string>> GetProductDetails(IEnumerable<string> Muids)
//        {
//            var tasks = Muids.Select(id => GetProduct(id));
//            var data = await Task.WhenAll(tasks);

//            return data;
//        }
//        [HttpPost]
//        [Route("uploadinBatches")]
//        public async Task<IEnumerable<string>> GetUsersInParallelInWithBatches([FromForm] UploadFile obj)

//        {
//            List<string> output = new List<string>();
//            string[] accepted_files = { "text/csv", "application/octet-stream" };
//            string ext = obj.File.ContentType;
//            var file_content = obj.File.OpenReadStream;


//            if (ext != "text/csv")
//            {
//                return null;
//            }

//            if (obj.File.Length > 0)
//            {
//                try
//                {
//                    StreamReader reader = new StreamReader(obj.File.OpenReadStream());


//                    string muid_line;
//                    List<string> Muid_list = new List<string>();

//                    while ((muid_line = reader.ReadLine()) != null)
//                    {
//                        if (muid_line != "")
//                            Muid_list.Add(muid_line);
//                    }
//                    var tasks = new List<Task<IEnumerable<string>>>();
//                    var batchSize = Environment.ProcessorCount;
//                    int numberOfBatches = (int)Math.Ceiling((double)Muid_list.Count() / batchSize);

//                    for (int i = 0; i < numberOfBatches; i++)
//                    {
//                        var currentIds = Muid_list.Skip(i * batchSize).Take(batchSize);
//                        tasks.Add(GetProductDetails(currentIds));
//                    }

//                    return (await Task.WhenAll(tasks)).SelectMany(u => u);
//                }
//                catch (Exception ex)
//                {
//                      return null;
//                }
//            }
//            else
//            {
//               return null;
//            }
//        } }

//    }

