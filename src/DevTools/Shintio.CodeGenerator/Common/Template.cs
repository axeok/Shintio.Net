using Shintio.CodeGenerator.Enums;
using Shintio.CodeGenerator.Interfaces;
using Shintio.CodeGenerator.Models;

namespace Shintio.CodeGenerator.Common;

public abstract class Template : ITemplate
{
    public abstract ProjectInfo ProjectInfo { get; }
    public abstract CodeLanguage CodeLanguage { get; }
    public abstract Task<IEnumerable<KeyValuePair<string, string>>> Run();
}