using System;

namespace LinkChecker.Domain
{
    public class Link
    {
        public Link(string url)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public string Url { get; }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null: return false;
                case Link other: return this.Url == other.Url;
                default: return false;
            }
        }

        public override int GetHashCode() => Url.GetHashCode();

        public override string ToString() => Url;
    }
}
