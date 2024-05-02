namespace ApiDemo.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequiredClaimAttribute : Attribute
{
    public string ClaimType { get; }
    public string ClaimValue { get; }
    // no setter, b/c we initialize value via constructor

    public RequiredClaimAttribute(string claimType, string claimValue)
    {
        ClaimType = claimType;
        ClaimValue = claimValue;
    }
}