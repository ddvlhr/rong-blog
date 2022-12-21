using Rong.Core.Enums;

namespace Rong.Infra.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute: Attribute
{
    public AutoInjectAttribute(Type interfaceType, InjectType injectType)
    {
        Type = interfaceType;
        InjectType = injectType;
    }

    public Type Type { get; set; }
    public InjectType InjectType { get; set; }
}