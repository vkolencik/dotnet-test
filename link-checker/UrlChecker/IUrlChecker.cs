using UrlChecker.Domain;

namespace UrlChecker
{
    public interface IUrlChecker
    {
        UrlCheckResult Check(UrlCheckerInput input);
    }
}
