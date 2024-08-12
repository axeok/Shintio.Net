using System.Collections.ObjectModel;
using System.Reflection;

namespace Shintio.Essentials.Extensions.ReflectionExtensions;

public static class NullableExtensions
{
    private const string NullableAttributeName = "System.Runtime.CompilerServices.NullableAttribute";
    private const string NullableContextAttributeName = "System.Runtime.CompilerServices.NullableContextAttribute";

    public static bool IsNeedNullableSuffix(this PropertyInfo property) =>
        property.IsNullable() && !(property.PropertyType.IsValueType &&
                                   Nullable.GetUnderlyingType(property.PropertyType) != null);

    public static bool IsNullable(this PropertyInfo property) =>
        IsNullable(property.PropertyType, property.DeclaringType, property.CustomAttributes);

    public static bool IsNullable(this ParameterInfo parameter) =>
        IsNullable(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);

    private static bool IsNullable(Type memberType, MemberInfo? declaringType,
        IEnumerable<CustomAttributeData> customAttributes)
    {
        if (memberType.IsValueType)
        {
            return Nullable.GetUnderlyingType(memberType) != null;
        }

        if (declaringType != null)
        {
            var result = HasCorrectNullableAttribute(customAttributes);
            if (result != null)
            {
                return result.Value;
            }

            result = HasCorrectNullableContextAttribute(declaringType);
            if (result != null)
            {
                return result.Value;
            }
        }

        // Couldn't find a suitable attribute
        return false;
    }

    private static bool? HasCorrectNullableAttribute(IEnumerable<CustomAttributeData> customAttributes)
    {
        var nullable =
            customAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableAttributeName);
        if (nullable is { ConstructorArguments.Count: 1 })
        {
            var attributeArgument = nullable.ConstructorArguments[0];
            if (attributeArgument.ArgumentType == typeof(byte[]))
            {
                var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
                if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                {
                    return (byte)args[0].Value! == 2;
                }
            }
            else if (attributeArgument.ArgumentType == typeof(byte))
            {
                return (byte)attributeArgument.Value! == 2;
            }
        }

        return null;
    }

    private static bool? HasCorrectNullableContextAttribute(MemberInfo declaringType)
    {
        for (var type = declaringType; type != null; type = type.DeclaringType)
        {
            var context = type.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == NullableContextAttributeName);
            if (context is { ConstructorArguments.Count: 1 } &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte)context.ConstructorArguments[0].Value! == 2;
            }
        }

        return null;
    }
}