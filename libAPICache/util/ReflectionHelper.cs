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
                .Where(x => x.PropertyType.IsGenericType);
        }
        public static IEnumerable<PropertyInfo> GetEnumerables(this PropertyInfo[] properties)
        {
            return properties.Where(x => x.PropertyType.IsGenericType);
        }
    }
}