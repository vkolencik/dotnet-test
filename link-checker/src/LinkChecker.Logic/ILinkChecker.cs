using LinkChecker.Domain;

namespace LinkChecker.Logic
{
    public interface ILinkChecker
    {
        LinkCheckResult Check(LinkCheckerInput input);
    }
}
