using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutlookHacker.Models.ConstStr
{
    public static class RegexStr
    {
        public const string MATCH_GUID = "pk=([A-z0-9]+-[A-z0-9]+-[A-z0-9]+-[A-z0-9]+-[A-z0-9]+)\\|";
        public const string MATCH_SURL = "surl=([[:alnum:][:punct:]]+)\\|";

        public const string MATCH_IP = 
            "\"(((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})(\\.((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})){3}:[[:digit:]]+)\"";
    }
}