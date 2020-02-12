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

        public LinkChecker() : this(new HttpClient()) { }

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
            bool isWellFormatted = Uri.TryCreate(link.Url, UriKind.Absolute, out Uri uri);
            if (!isWellFormatted)
            {
                return LinkStatus.MALFORMED;
            }

            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                return LinkStatus.SKIPPED;
            }

            try
            {
                var request = _httpClient.GetAsync(uri);
                request.Wait();
                return request.Result.IsSuccessStatusCode ? LinkStatus.OK : LinkStatus.INVALID;
            }
            catch (HttpRequestException)
            {
                return LinkStatus.INVALID;
            }
        }
    }
}
