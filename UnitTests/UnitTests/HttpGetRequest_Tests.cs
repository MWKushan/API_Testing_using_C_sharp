using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace UnitTests
{
    [TestClass]
    public class HttpGetRequest_Tests
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
        public void HttpGetRequest_ListAllObjects_ValidateCount()
        {
            var requestUrl = "https://api.restful-api.dev/objects";
            var httpReq = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var responseContent = string.Empty; // Declare a varaible to store response content

            var response = httpClient.SendAsync(httpReq).Result;            
            responseContent = response.Content.ReadAsStringAsync().Result; // Convert to the content to the string            
            var result = JsonConvert.DeserializeObject<List<Info>>(responseContent); // desirilized the conetnt to match with expected results

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(result.Count, 13); // Validate total object count in the response
        }


        [TestMethod]
        public void HttpGetRequest_ListAllObjects_ValidateObjects() // Validate object of the response body
        {
            var requestUrl = "https://api.restful-api.dev/objects";
            var httpReq = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var responseContent = string.Empty; // Declare a varaible to store response content

            var response = httpClient.SendAsync(httpReq).Result;
            responseContent = response.Content.ReadAsStringAsync().Result; // Convert to the content to the string            
            var result = JsonConvert.DeserializeObject<List<Info>>(responseContent); // desirilized the conetnt to match with expected results

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK); // Validate response code is 200
            
            // validate first object
            Assert.AreEqual("1", result[0].id);
            Assert.AreEqual("Google Pixel 6 Pro", result[0].name);
            Assert.AreEqual("Cloudy White", result[0].data.color);
            Assert.AreEqual("128 GB", result[0].data.capacity);
            Assert.AreEqual(result[0].data.capacity, "128 GB");

            // Validate 2nd object
            Assert.AreEqual("2", result[1].id);
            Assert.AreEqual("Apple iPhone 12 Mini, 256GB, Blue", result[1].name);
            Assert.AreEqual(null, result[1].data);

            // Like this we can validate the all the objects.
           
        }



        [TestMethod]
        public void HttpGetRequest_ListObjectsById_ValidateContent()
        {
            var requestUrl = "https://api.restful-api.dev/objects?id=3&id=5&id=10";
            var httpReq = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var responseContent = string.Empty;

            var response = httpClient.SendAsync(httpReq).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<Info>>(responseContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(result.Count, 3); // validate the total count of the response is 3

            // validate first object of the response
            Assert.AreEqual("3", result[0].id);
            Assert.AreEqual("Apple iPhone 12 Pro Max", result[0].name);
            Assert.AreEqual("Cloudy White", result[0].data.color);
            Assert.AreEqual("512", result[0].data.capacityGb);

            // Validate second object of the response
            Assert.AreEqual("5", result[1].id);
            Assert.AreEqual("Samsung Galaxy Z Fold2", result[1].name);
            Assert.AreEqual("689.99", result[1].data.price);
            Assert.AreEqual("Brown", result[1].data.color);

            // Validate 3rd object of the response
            Assert.AreEqual("10", result[2].id);
            Assert.AreEqual("Apple iPad Mini 5th Gen", result[2].name);
            Assert.AreEqual("64 GB", result[2].data.capacity);
            Assert.AreEqual("7.9", result[2].data.screenSize);
        }

        [TestMethod]
        public void HttpGetRequest_SingleObject_ValidateContent()
        {
            var requestUrl = "https://api.restful-api.dev/objects/7";
            var httpReq = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var responseContent = string.Empty;

            var response = httpClient.SendAsync(httpReq).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Info>(responseContent);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            // Response has only one object, so we can access directly without a array
            Assert.AreEqual("7", result.id);
            Assert.AreEqual("Apple MacBook Pro 16", result.name);
            Assert.AreEqual("2019", result.data.year);
            Assert.AreEqual("1849.99", result.data.price);
            Assert.AreEqual("Intel Core i9", result.data.cpuModel);
            Assert.AreEqual("1 TB", result.data.hardDiskSize);

        }
    }
}
