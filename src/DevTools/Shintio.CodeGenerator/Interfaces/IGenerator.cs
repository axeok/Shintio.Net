using Shintio.CodeGenerator.Models;

namespace Shintio.CodeGenerator.Interfaces;

public interface IGenerator
{
    public Task Load();
    public Task<IEnumerable<FileResult>> Run();
}