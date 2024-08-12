using Shintio.Essentials.Common;

namespace Shintio.CodeGenerator.Models;

public class ProjectInfo : ValueObject
{
    public ProjectInfo(string path, bool combineCode)
    {
        Path = path;
        CombineCode = combineCode;
    }

    public string Path { get; }
    public bool CombineCode { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Path;
        yield return CombineCode;
    }
}