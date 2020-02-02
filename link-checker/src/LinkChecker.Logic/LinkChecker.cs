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
            var linkStates = new Dictionary<Link, LinkStatus>();

            foreach (var link in input.Links) 
            {
                linkStates.Add(link, CheckLink(link));
            }
            
            return new LinkCheckResult(linkStates);
        }

        private LinkStatus CheckLink(Link link)
        {
            bool wellFormattedLink = Uri.TryCreate(link.Url, UriKind.Absolute, out Uri uri);
            if (!wellFormattedLink)
            {
                return LinkStatus.MALFORMED;
            }

            return LinkStatus.OK;

        }
    }
}
