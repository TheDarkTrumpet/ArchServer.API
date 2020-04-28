using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using libAPICache.Models;

namespace libAPICache.util
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetEnumerables(this Base cl)
        {
            return cl.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                .Where(x => x.PropertyType.IsGenericType && x.HasEnumerable());
        }
        public static bool HasEnumerable(this PropertyInfo property)
        {
            return property.PropertyType.IsGenericType &&
                   property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable));
        }
    }
}