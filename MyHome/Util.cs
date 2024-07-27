using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public static class Util
    {
        public static string BuildUrlWithQueryStringUsingStringConcat(string basePath, Dictionary<string, string> queryParams)
        {
            var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}+{kvp.Value}+manual"));
            return $"{basePath}{queryString}";
        }

    }
}
