using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkChecker.Domain {
    public class LinkCheckerInput 
    {
        public LinkCheckerInput(IEnumerable<Link> links) 
        {
            if (links == null)
            {
                throw new ArgumentNullException(nameof(links));
            }
            this.Links = links.ToList().AsReadOnly();
        }

        public IEnumerable<Link> Links { get; }
    }
}