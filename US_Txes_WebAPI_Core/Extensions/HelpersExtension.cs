using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace US_Txes_WebAPI_Core.Extensions
{
    public static class HelpersExtension
    {
        public static bool IsAny<T>(this IEnumerable<T> collection)
        {
            if (collection != null && collection.Any())
                return true;

            return false;
        }
    }
}
