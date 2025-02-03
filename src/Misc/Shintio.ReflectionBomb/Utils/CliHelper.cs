using System.Diagnostics;
using System.Threading.Tasks;
using Shintio.ReflectionBomb.Enums;
using Shintio.ReflectionBomb.Types;

namespace Shintio.ReflectionBomb.Utils
{
    public static class CliHelper
    {
        public static async Task<string> GetOutput(string command, CliInterpreter interpreter = CliInterpreter.Cmd)
        {
            var process = ProcessWrapper.Start(new ProcessStartInfo
            {
                FileName = GetFileName(interpreter),
                Arguments = FormatArguments(command, interpreter),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            });

            process.Start();

            return await process.StandardOutput.ReadToEndAsync();
        }

        private static string GetFileName(CliInterpreter interpreter) => interpreter switch
        {
            CliInterpreter.Cmd => "cmd.exe",
            CliInterpreter.Powershell => "powershell",
            _ => "",
        };

        private static string FormatArguments(string command, CliInterpreter interpreter) => interpreter switch
        {
            CliInterpreter.Cmd => $"/C {command}",
            CliInterpreter.Powershell => $"-NoProfile -NonInteractive -Command \"{command}\"",
            _ => command,
        };
    }
}