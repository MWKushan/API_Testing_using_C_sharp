using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class HttpDeleteRequest_Tests
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
        public void HttpDeleteREquest_AddObject_Validate()
        {
            var requestUrl = "https://api.restful-api.dev/objects";

            var itemToPost = new Info()
            {
                name = "Apple MacBook Pro 16",
                data = new Data()
                {
                    year = "2019",
                    price = "2049.99",
                    cpuModel = "Intel Core i9",
                    hardDiskSize = "1 TB",
                    color= "silver"

                }
            };

            var httpReq = new HttpRequestMessage(HttpMethod.Post, requestUrl);          
            var content = new StringContent(JsonConvert.SerializeObject(itemToPost), Encoding.UTF8, "application/json");
            httpReq.Content = content;

            var responseContent = string.Empty;

            var response = httpClient.SendAsync(httpReq).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<Info>(responseContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            var newId = result.id;


            //DELETE newly generated Object

            var requestDeleteUrl = "https://api.restful-api.dev/objects/"+newId;
          
            var httpReq2 = new HttpRequestMessage(HttpMethod.Delete, requestDeleteUrl);

            var responseContentDelete = string.Empty;

            var responseDelete = httpClient.SendAsync(httpReq2).Result;
            responseContent = responseDelete.Content.ReadAsStringAsync().Result;

            var deleteResponse = JsonConvert.DeserializeObject<DeleteResponse>(responseContent);

            Assert.AreEqual(responseDelete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual($"Object with id = {newId} has been deleted.", deleteResponse.message);
        }
    }
}
