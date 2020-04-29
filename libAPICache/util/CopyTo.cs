using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace libAPICache.util
{
    public static class CopyFrom
    {
        public static T1 Copy<T1, T2>(this T1 obj, T2 otherObject)
            where T1: class
            where T2: class
        {
            IEnumerable<PropertyInfo> srcFields = otherObject.GetType().GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                .Where(x => !x.PropertyType.IsGenericType);

            IEnumerable<PropertyInfo> destFields = obj.GetType().GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                .Where(x => !x.PropertyType.IsGenericType && x.HasEnumerable());

            foreach (var property in srcFields) {
                var dest = destFields.FirstOrDefault(x => x.Name == property.Name);

                if (dest != null && dest.CanWrite)
                {
                    dest.SetValue(obj, property.GetValue(otherObject, null), null);
                }
                    
            }

            return obj;
        }
    }
}