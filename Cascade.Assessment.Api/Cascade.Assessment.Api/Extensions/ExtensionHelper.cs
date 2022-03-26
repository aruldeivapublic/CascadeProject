using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cascade.Assessment.Api.Extensions
{
    public static class ExtensionHelper
    {
        public static string ToCamelCase(this string input)
        {
            input = input.Trim();
            var result = input.Substring(0, 1).ToUpper();
            if(input.Length > 1)
            {
                result += input.Substring(1).ToLower();
            }
            return result;
        }
    }
}
