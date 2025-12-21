using System;
using System.Collections.Generic;
using System.Text;

namespace CnE2PLC.Helpers
{
    public static class StringHelper
    {
        public static int ContainsAsBit(this string? str, string searchStr)
        {
            return (str ?? String.Empty).Contains(searchStr) ? 1 : 0;
        }

        public static T? ParseEnum<T>(this string str) where T : struct, Enum
        {
            return Enum.TryParse<T>(str, out var t) ? t : (T?)null;
        }
    }
}
