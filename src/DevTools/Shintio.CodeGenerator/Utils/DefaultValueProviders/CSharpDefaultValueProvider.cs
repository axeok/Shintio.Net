using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using Shintio.CodeGenerator.Extensions;
using Shintio.Essentials.Common;

namespace Shintio.CodeGenerator.Utils.DefaultValueProviders;

public class CSharpDefaultValueProvider : BaseDefaultValueProvider
{
    protected readonly string ReadOnlyCollectionTypeName =
        ReflectionHelper.TrimGenericName(typeof(ReadOnlyCollection<>).FullName!);

    protected readonly string ListTypeName =
        ReflectionHelper.TrimGenericName(typeof(List<>).FullName!);

    public override string Get(PropertyInfo property)
    {
        var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
        if (defaultValueAttribute != null)
        {
            return GeneratorHelper.FormatPropertyValue(property, defaultValueAttribute.Value);
        }

        var defaultValue = Essentials.Utils.ReflectionHelper.GetPropertyValue(property);
        if (defaultValue != null)
        {
            return GeneratorHelper.FormatPropertyValue(property,
                JsonSerializer.Serialize(defaultValue).Replace("\"", "\\\""));
        }

        return base.Get(property);
    }

    public override string Get(Type type, bool isNullable)
    {
        if (isNullable)
        {
            return "null";
        }

        if (type == typeof(string))
        {
            return "string.Empty";
        }

        // if (type.IsSubclassOf(typeof(DataCollection)))
        if (type.BaseType?.Name == "DataCollection")
        {
            return "null!";
        }

        var typeString = type.GetTypeString();

        if (type.IsPrimitive || type.IsEnum)
        {
            return $"default({typeString})";
        }

        if (type.FullName!.Contains(ReadOnlyCollectionTypeName))
        {
            return $"new {typeString.Replace(ReadOnlyCollectionTypeName, ListTypeName)}().AsReadOnly()";
        }

        return $"new {typeString}()";
    }
}