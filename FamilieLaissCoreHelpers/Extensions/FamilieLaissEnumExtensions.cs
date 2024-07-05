using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissCoreHelpers.Extensions
{
    public static class FamilieLaissEnumExtensions
    {
        public static T ToEnum<T>(this int enumValue)
        {
            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        public static T ToEnum<T>(this long enumValue)
        {
            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        public static T ToEnum<T>(this byte enumValue)
        {
            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        public static T ToEnum<T>(this short enumValue)
        {
            return (T)Enum.ToObject(typeof(T), enumValue);
        }
    }
}
