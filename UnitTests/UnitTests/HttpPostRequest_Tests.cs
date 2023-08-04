using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class HttpPostRequest_Tests
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
        public void HttpPostREquest_AddObject_Validate()
        {
            var requestUrl = "https://api.restful-api.dev/objects";

            var itemToPost = new Info() // Declare varaible to pass as new object
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

            var httpReq = new HttpRequestMessage(HttpMethod.Post, requestUrl); // Declare a variable to Pass the post call request url
           
            var content = new StringContent(JsonConvert.SerializeObject(itemToPost), Encoding.UTF8, "application/json"); // Serialize the post call body paramter before pass it
            httpReq.Content = content;

            var responseContent = string.Empty;

            var response = httpClient.SendAsync(httpReq).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<Info>(responseContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            Assert.AreEqual("Apple MacBook Pro 16", result.name);
            Assert.AreEqual("2019", result.data.year);
            Assert.AreEqual("1849.99", result.data.price);
            Assert.AreEqual("Intel Core i9", result.data.cpuModel);
            Assert.AreEqual("1 TB", result.data.hardDiskSize);
        }
    }
}
