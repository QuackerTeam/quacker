using System;

namespace Quacker.Common.Helpers.Extensions
{
    public static class TypeExtensions
    {
        /*
         http://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        */

        public static bool IsSubclassOfRawGeneric(this Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == cur)
                    return true;
                type = type.BaseType;
            }
            return false;
        }
    }
}