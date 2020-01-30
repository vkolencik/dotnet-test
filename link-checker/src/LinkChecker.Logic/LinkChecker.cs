using System;
using System.Collections.Generic;
using System.Net.Http;

using LinkChecker.Domain;

namespace LinkChecker.Logic
{
    public class LinkChecker : ILinkChecker
    {
        private HttpClient _httpClient;

        public LinkChecker(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public LinkCheckResult Check(LinkCheckerInput input) 
        {
            var linkStates = new Dictionary<Uri, LinkStatus>();

            foreach (var link in input.Links) 
            {
                linkStates.Add(link, LinkStatus.OK);
            }
            
            return new LinkCheckResult(linkStates);
        }
    }
}
