using Profile.Domain.Models;

namespace Profile.Domain.Specifications.ProfileSpecifications;

public class ContainsInterestSpecification : ExpressionSpecification<UserProfile>
{
    public ContainsInterestSpecification(int interestId) : base(p => p.Interests.Any(i => i.Id == interestId))
    {
    }
}