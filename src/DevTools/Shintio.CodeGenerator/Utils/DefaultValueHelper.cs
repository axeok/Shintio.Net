using System.Reflection;
using Shintio.CodeGenerator.Enums;
using Shintio.CodeGenerator.Utils.DefaultValueProviders;

namespace Shintio.CodeGenerator.Utils;

public class DefaultValueHelper
{
    private static readonly Dictionary<CodeLanguage, BaseDefaultValueProvider> LangToProvider = new()
    {
        [CodeLanguage.CSharp] = new CSharpDefaultValueProvider(),
        [CodeLanguage.JavaScript] = new JavaScriptDefaultValueProvider(),
    };

    public static string Get(PropertyInfo propertyInfo, CodeLanguage language)
    {
        if (!LangToProvider.TryGetValue(language, out var provider))
        {
            return $"default({propertyInfo.PropertyType})";
        }

        return provider.Get(propertyInfo);
    }
}