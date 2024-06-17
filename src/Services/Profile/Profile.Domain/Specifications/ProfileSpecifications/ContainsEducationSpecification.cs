using Profile.Domain.Models;

namespace Profile.Domain.Specifications.ProfileSpecifications;

public class ContainsEducationSpecification : ExpressionSpecification<UserProfile>
{
    public ContainsEducationSpecification(int educationId) : base(p => p.UserEducations.Any(i => i.EducationId == educationId))
    {
    }
}