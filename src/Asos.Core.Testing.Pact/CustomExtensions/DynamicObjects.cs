using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using PactNet.Matchers;

namespace Asos.Core.Testing.Pact.CustomExtensions
{
    public static class DynamicObjects
    {
        public static dynamic ToDynamicResponse<T>(this T obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                ICollection list = null;
                var value = property.GetValue(obj);
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && !typeof(string).IsAssignableFrom(property.PropertyType))
                {
                    list = (ICollection)value;
                }
                if (list == null && value != null)
                {
                    expando.Add(property.Name, Match.Type(value));
                }
                if (list != null && list.Count > 0)
                {
                    expando.Add(property.Name, Match.Type(value));
                }
            }
            return (ExpandoObject)expando;
        }

        public static dynamic ToDynamicRequest<T>(this T obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                expando.Add(property.Name, property.GetValue(obj));
            }
            return (ExpandoObject)expando;
        }
    }
}
