using Profile.Domain.Models;

namespace Profile.Domain.Specifications.ProfileSpecifications;

public class InterestsLessThanSpecification : ExpressionSpecification<UserProfile>
{
    public InterestsLessThanSpecification(int lessThanCount) : base(p=> p.Interests.Count < lessThanCount)
    {
    }
}