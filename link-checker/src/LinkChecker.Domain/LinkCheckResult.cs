using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LinkChecker.Domain 
{
    public class LinkCheckResult
    {
        public LinkCheckResult(IDictionary<Uri, LinkStatus> linkStates) 
        {
            this.LinkStates = new ReadOnlyDictionary<Uri, LinkStatus>(linkStates);
        }

        public IDictionary<Uri, LinkStatus> LinkStates { get; }
    }
}