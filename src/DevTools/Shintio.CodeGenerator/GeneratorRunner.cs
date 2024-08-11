using System.Collections.Concurrent;
using System.Diagnostics;
using Shintio.CodeGenerator.Enums;
using Shintio.CodeGenerator.Interfaces;
using Shintio.CodeGenerator.Models;
using Shintio.CodeProcessor.Utils;

namespace Shintio.CodeGenerator;

public class GeneratorRunner
{
    public static event Action? Clear;

    public static async Task Run(params IEnumerable<IGenerator>[] generatorsChunks)
    {
        Clear?.Invoke();

        var files = new ConcurrentBag<FileResult>();

        foreach (var generators in generatorsChunks)
        {
            await Parallel.ForEachAsync(generators, async (generator, _) => await generator.Load());
        }

        foreach (var generators in generatorsChunks)
        {
            await Parallel.ForEachAsync(generators, async (generator, _) =>
            {
                var result = await generator.Run();
                foreach (var file in result)
                {
                    files.Add(file);
                }
            });
        }

        SaveFiles(files);
    }

    private static void SaveFiles(IEnumerable<FileResult> files)
    {
        var stopwatch = Stopwatch.StartNew();
        var savedFiles = 0;

        foreach (var projectFiles in files.GroupBy(f => f.Project))
        {
            var project = projectFiles.Key;

            var filesToSave = project.CombineCode && false
                ? [CombineCode(project, projectFiles)]
                : projectFiles.ToArray();

            foreach (var file in filesToSave)
            {
                var path = Path.Combine(project.Path, "GeneratedCode", file.Name);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                if (File.Exists(path) && File.ReadAllText(path) == file.Content)
                {
                    continue;
                }

                File.WriteAllText(path, file.Content);
                savedFiles++;
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"Saved {savedFiles} files in {stopwatch.Elapsed}");
    }

    private static FileResult CombineCode(ProjectInfo project, IEnumerable<FileResult> files)
    {
        return new FileResult(
            "Combined.",
            project,
            CodeLanguage.CSharp,
            SharpCombiner.CombineCode(files.Select(f => f.Content))
        );
    }
}