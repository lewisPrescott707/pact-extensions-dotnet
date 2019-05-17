using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Testing.Pact.CustomExtensions
{
    public class JsonResolver
    {
        public class SkipEmptyContractResolver : CamelCasePropertyNamesContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.ShouldSerialize = obj =>
                {
                    if (property.PropertyType.Name.Contains("ICollection"))
                    {
                        return (property.ValueProvider.GetValue(obj) as dynamic).Count > 0;
                    }
                    return true;
                };
                return property;
            }
        }
    }
}
