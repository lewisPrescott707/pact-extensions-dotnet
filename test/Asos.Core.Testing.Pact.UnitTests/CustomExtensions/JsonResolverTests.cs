using System.Collections;
using System.Collections.ObjectModel;
using Asos.Core.Testing.Pact.CustomExtensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Asos.Core.Testing.Pact.UnitTests.CustomExtensions
{
    [TestFixture]
    public class JsonResolverTests
    {
        [Test]
        public void When_Create_Property_With_Empty_ICollection__Then_Remove_From_Json()
        {
            var jsonResolver = new JsonResolver.SkipEmptyContractResolver();

            var json = JsonConvert.SerializeObject(new TestJson()
            {
                Id = 1,
                Names = new Collection<string>()
            }, new JsonSerializerSettings()
            {
                ContractResolver = jsonResolver
            });

            Assert.AreEqual("{\"id\":1}", json);
        }

        [Test]
        public void When_Create_Property_With_Collection__Then_Return_In_Json()
        {
            var jsonResolver = new JsonResolver.SkipEmptyContractResolver();

            var json = JsonConvert.SerializeObject(new TestJson()
            {
                Id = 1,
                Names = new Collection<string> { "dave" }
            }, new JsonSerializerSettings()
            {
                ContractResolver = jsonResolver
            });

            Assert.AreEqual("{\"id\":1,\"names\":[\"dave\"]}", json);
        }

        public class TestJson
        {
            public int Id;
            public ICollection Names;
        }
    }
}
