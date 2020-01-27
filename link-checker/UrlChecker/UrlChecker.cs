using System;
using System.Collections.Generic;
using UrlChecker.Domain;

namespace UrlChecker
{
    public class UrlChecker : IUrlChecker
    {
        public UrlCheckResult Check(UrlCheckerInput input) 
        {
            var urlStates = new Dictionary<Uri, UrlStatus>();

            foreach (var url in input.Urls) 
            {
                urlStates.Add(url, UrlStatus.OK);
            }
            
            return new UrlCheckResult(urlStates);
        }
    }
}
