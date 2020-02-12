using System;
using System.IO;
using System.Collections.Generic;

using LinkChecker.Domain;

namespace LinkChecker.Cli
{
    public class LinkReader : IDisposable
    {
        private StreamReader _reader;
        public LinkReader(Stream stream)
        {
            _reader = new StreamReader(stream ?? throw new ArgumentNullException(nameof(stream)));
        }

        public IEnumerable<Link> Read()
        {
            while (_reader.Peek() >= 0)
            {
                yield return new Link(_reader.ReadLine());
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
