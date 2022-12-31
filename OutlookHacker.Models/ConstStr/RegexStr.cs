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
    }
}