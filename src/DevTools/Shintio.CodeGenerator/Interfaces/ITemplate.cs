using Shintio.CodeGenerator.Enums;
using Shintio.CodeGenerator.Models;

namespace Shintio.CodeGenerator.Interfaces;

public interface ITemplate
{
    public ProjectInfo ProjectInfo { get; }
    public CodeLanguage CodeLanguage { get; }
    public Task<IEnumerable<KeyValuePair<string, string>>> Run();
}