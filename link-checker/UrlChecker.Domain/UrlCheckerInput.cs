using System;
using System.Collections.Generic;
using System.Linq;

namespace UrlChecker.Domain {
    public class UrlCheckerInput 
    {
        public UrlCheckerInput(IEnumerable<Uri> urls) 
        {
            this.Urls = urls.ToList().AsReadOnly();
        }

        public IEnumerable<Uri> Urls { get; }
    }
}