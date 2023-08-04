using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class HttpPutRequest_Tests
    {
        private HttpClient httpClient;

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(100)
            };
        }

        [TestMethod]
        public void HttpPutREquest_AddObject_Validate()
        {
            var requestUrl = "https://api.restful-api.dev/objects";

            var itemToPost = new Info()
            {
                name = "Apple MacBook Pro 16",
                data = new Data()
                {
                    year = "2019",
                    price = "1849.99",
                    cpuModel = "Intel Core i9",
                    hardDiskSize = "1 TB"
                  

                }
            };

            //1st time calling the post call and create a object
            var httpReq = new HttpRequestMessage(HttpMethod.Post, requestUrl);            

            var checkSerilize = JsonConvert.SerializeObject(itemToPost);
            var content = new StringContent(JsonConvert.SerializeObject(itemToPost), Encoding.UTF8, "application/json");
            httpReq.Content = content;

            var responseContent = string.Empty;

            var response = httpClient.SendAsync(httpReq).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<Info>(responseContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            var newId = result.id; // Capture the created new object id and save it to a paramter to pass in url


            //PUT newly generated Object

            var requestPutUrl = "https://api.restful-api.dev/objects/"+newId;

            // Change price and add color varaible and create a paramter for PUT call
            var itemToPut = new Info()
            {
                name = "Apple MacBook Pro 16",
                data = new Data() 
                {
                    year = "2019",
                    price = "2049.99",
                    cpuModel = "Intel Core i9",
                    hardDiskSize = "1 TB",
                    color = "silver"

                }
            };

            var httpReq2 = new HttpRequestMessage(HttpMethod.Put, requestPutUrl);

            var putContent = new StringContent(JsonConvert.SerializeObject(itemToPut), Encoding.UTF8, "application/json");  //Serialize object 'itemToPut' before do the PUT call
            httpReq2.Content = putContent;

            var responseContentPut = string.Empty;

            var responsePut = httpClient.SendAsync(httpReq2).Result;
            responseContentPut = responsePut.Content.ReadAsStringAsync().Result;

            var resultPut = JsonConvert.DeserializeObject<Info>(responseContentPut);

            Assert.AreEqual(responsePut.StatusCode, HttpStatusCode.OK); //Validate response code

            Assert.AreEqual("Apple MacBook Pro 16", resultPut.name);
           Assert.AreEqual("2019", resultPut.data.year);
            Assert.AreEqual("2049.99", resultPut.data.price);
           Assert.AreEqual("Intel Core i9", resultPut.data.cpuModel);
            Assert.AreEqual("1 TB", resultPut.data.hardDiskSize);
            Assert.AreEqual("silver", resultPut.data.color);
        }
    }
}
