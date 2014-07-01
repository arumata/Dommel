using System;
using System.Collections.Generic;
using System.Linq;

namespace Dommel.Linq.Utils
{
    internal static class TypeHelper
    {
        internal static Type GetElementType(Type seqType)
        {
            Type enumerable = FindIEnumerable(seqType);
            return enumerable == null ? seqType : enumerable.GetGenericArguments()[0];
        }

        private static Type FindIEnumerable(Type type)
        {
            if (type == null || type == typeof(string))
            {
                return null;
            }

            if (type.IsArray)
            {
                return typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
            }

            if (type.IsGenericType)
            {
                foreach (Type arg in type.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(type))
                    {
                        return ienum;
                    }
                }
            }

            Type[] interfaces = type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                foreach (Type enumerable in interfaces.Select(FindIEnumerable).Where(ienum => ienum != null))
                {
                    return enumerable;
                }
            }

            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                return FindIEnumerable(type.BaseType);
            }

            return null;
        }
    }
}
