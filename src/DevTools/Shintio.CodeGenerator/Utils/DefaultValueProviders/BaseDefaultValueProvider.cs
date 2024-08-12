using System.Reflection;
using Shintio.Essentials.Extensions.ReflectionExtensions;

namespace Shintio.CodeGenerator.Utils.DefaultValueProviders;

public abstract class BaseDefaultValueProvider
{
    public virtual string Get(PropertyInfo property) => Get(property.PropertyType, property.IsNullable());

    public abstract string Get(Type type, bool isNullable);
}