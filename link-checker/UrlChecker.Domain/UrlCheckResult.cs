using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UrlChecker.Domain 
{
    public class UrlCheckResult
    {
        public UrlCheckResult(IDictionary<Uri, UrlStatus> urlStates) 
        {
            this.UrlStates = new ReadOnlyDictionary<Uri, UrlStatus>(urlStates);
        }

        public IDictionary<Uri, UrlStatus> UrlStates { get; }
    }
}