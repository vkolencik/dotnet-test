using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LinkChecker.Domain 
{
    public class LinkCheckResult
    {
        public LinkCheckResult(IDictionary<Link, LinkStatus> linkStates) 
        {
            this.LinkStates = new ReadOnlyDictionary<Link, LinkStatus>(linkStates);
        }

        public IDictionary<Link, LinkStatus> LinkStates { get; }
    }
}