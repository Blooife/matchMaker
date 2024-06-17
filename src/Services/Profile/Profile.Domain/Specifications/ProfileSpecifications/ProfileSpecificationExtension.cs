using Profile.Domain.Models;

namespace Profile.Domain.Specifications.ProfileSpecifications;

public static class ProfileSpecificationExtension
{
    public static bool ContainsInterest(this UserProfile profile, int interestId)
    {
        var spec = new ContainsInterestSpecification(interestId);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
    
    public static bool InterestsLessThan(this UserProfile profile, int lessThanCount)
    {
        var spec = new InterestsLessThanSpecification(lessThanCount);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
    
    public static bool ContainsLanguage(this UserProfile profile, int languageId)
    {
        var spec = new ContainsInterestSpecification(languageId);
        bool result = spec.IsSatisfied(profile);
        return result;
    }
}