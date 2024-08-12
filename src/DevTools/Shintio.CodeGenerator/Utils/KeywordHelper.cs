using System.Collections.ObjectModel;
using Shintio.CodeGenerator.Enums;

namespace Shintio.CodeGenerator.Utils;

public class KeywordHelper
{
    private static readonly Dictionary<CodeLanguage, IReadOnlyCollection<string>> LanguageToKeywords = new()
    {
        [CodeLanguage.CSharp] = new[]
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
            "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
            "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
            "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
            "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof",
            "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while",
        },
        [CodeLanguage.JavaScript] = new[]
        {
            "abstract", "arguments", "await", "boolean", "break", "byte", "case", "catch",
            "char", "class", "cons", "continue", "debugger", "default", "delete", "do",
            "double", "else", "enum", "eval", "export", "extends", "false", "final",
            "finally", "float", "for", "function", "goto", "if", "implements", "import",
            "in", "instanceof", "int", "interface", "let", "long", "native", "new",
            "null", "package", "private", "protected", "public", "return", "short", "static",
            "super", "switch", "synchronized", "this", "throw", "throws", "transient", "true",
            "try", "typeof", "var", "void", "volatile", "while", "with", "yield"
        },
    };

    public static bool IsKeyword(string value, CodeLanguage language)
    {
        return LanguageToKeywords.TryGetValue(language, out var keywords) && keywords.Contains(value);
    }
}