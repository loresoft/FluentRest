#if NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
public sealed class AllowNullAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
public sealed class DisallowNullAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class MaybeNullAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class NotNullAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class MaybeNullWhenAttribute : Attribute
{
    public MaybeNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

    public bool ReturnValue { get; }
}

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class NotNullWhenAttribute : Attribute
{
    public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;

    public bool ReturnValue { get; }
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
public sealed class NotNullIfNotNullAttribute : Attribute
{
    public NotNullIfNotNullAttribute(string parameterName) => ParameterName = parameterName;

    public string ParameterName { get; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DoesNotReturnAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class DoesNotReturnIfAttribute : Attribute
{
    public DoesNotReturnIfAttribute(bool parameterValue) => ParameterValue = parameterValue;

    public bool ParameterValue { get; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class MemberNotNullAttribute : Attribute
{
    public MemberNotNullAttribute(string member) => Members = [member];

    public MemberNotNullAttribute(params string[] members) => Members = members;

    public string[] Members { get; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class MemberNotNullWhenAttribute : Attribute
{
    public MemberNotNullWhenAttribute(bool returnValue, string member)
    {
        ReturnValue = returnValue;
        Members = [member];
    }

    public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
    {
        ReturnValue = returnValue;
        Members = members;
    }

    public bool ReturnValue { get; }

    public string[] Members { get; }
}
#endif
