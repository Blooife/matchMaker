using Profile.Domain.Models;

namespace Profile.Domain.Specifications.ProfileSpecifications;

public class ContainsLanguageSpecification : ExpressionSpecification<UserProfile>
{
    public ContainsLanguageSpecification(int languageId) : base(p => p.Languages.Any(i => i.Id == languageId))
    {
    }
}