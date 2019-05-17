using System;
using System.Collections.Generic;
using System.Dynamic;
using Core.Testing.Pact.CustomExtensions;
using NUnit.Framework;
using PactNet.Matchers.Type;

namespace Core.Testing.Pact.UnitTests.CustomExtensions
{
    [TestFixture]
    public class DynamicObjectsTests
    {
        [Test]
        public void When_Dynamic_Response__Then_Response_Matchers()
        {
            var dateTime = DateTime.Today;
            var testResponse = new TestRequest()
            {
                Id = "id123",
                Date = dateTime,
                Age = 100
            };

            dynamic expectedResponse = new ExpandoObject();
            expectedResponse.Id = new TypeMatcher(testResponse.Id);
            expectedResponse.Age = new TypeMatcher(testResponse.Age);
            expectedResponse.Date = new TypeMatcher(testResponse.Date);

            var dynamicResponse = testResponse.ToDynamicResponse();
            Assert.AreEqual(expectedResponse.Id.Example, dynamicResponse.Id.Example);
            Assert.AreEqual(expectedResponse.Id.Match, dynamicResponse.Id.Match);
            Assert.AreEqual(expectedResponse.Age.Example, dynamicResponse.Age.Example);
            Assert.AreEqual(expectedResponse.Age.Match, dynamicResponse.Age.Match);
            Assert.AreEqual(expectedResponse.Date.Example, dynamicResponse.Date.Example);
            Assert.AreEqual(expectedResponse.Date.Match, dynamicResponse.Date.Match);
        }

        [Test]
        public void When_Dynamic_Response_Includes_IEnumerable___Then_Response_Collection()
        {
            var testResponse = new TestRequest()
            {
                List = new List<string>()
                {
                    "betty"
                }
            };

            dynamic expectedResponse = new ExpandoObject();
            expectedResponse.List = new TypeMatcher(testResponse.List);

            var dynamicResponse = testResponse.ToDynamicResponse();
            Assert.AreEqual(expectedResponse.List.Example, dynamicResponse.List.Example);
        }

        [Test]
        public void When_Dynamic_Request__Then_Request_Expando_Object()
        {
            var dateTime = DateTime.Today;
            var testRequest = new TestRequest()
            {
                Id = "id123",
                Date = dateTime,
                Age = 100
            };

            dynamic expectedRequest = new ExpandoObject();
            expectedRequest.Id = "id123";
            expectedRequest.Date = dateTime;
            expectedRequest.Age = 100;
            expectedRequest.List = null;

            Assert.AreEqual(expectedRequest, testRequest.ToDynamicRequest());
        }
    }

    public class TestRequest
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int Age { get; set; }
        public IEnumerable<string> List { get; set; }
    }
}
