using System;
using System.Threading.Tasks;
using Shintio.Essentials.Interfaces;

namespace Shintio.Essentials.Utils
{
    public class ConsoleOutput : IOutput
    {
        private OutputProgress? _currentProgress;

        public Task Write(string message)
    {
        Console.Write(message);

        return Task.CompletedTask;
    }

        public Task WriteLine(string message)
    {
        Console.WriteLine(message);

        return Task.CompletedTask;
    }

        public Task<OutputProgress> CreateProgress(string title, double max)
    {
        _currentProgress?.Stop();

        _currentProgress = new OutputProgress(title, max);

        _currentProgress.ValueUpdated += CurrentProgressOnValueUpdated;
        _currentProgress.Stopped += CurrentProgressOnStopped;

        return Task.FromResult(_currentProgress);
    }

        private void CurrentProgressOnValueUpdated(OutputProgress progress)
    {
        Console.WriteLine("{0}: {1}/{2}", progress.Title, progress.Value, progress.Max);
    }

        private void CurrentProgressOnStopped(OutputProgress progress)
    {
        progress.ValueUpdated -= CurrentProgressOnValueUpdated;
        progress.Stopped -= CurrentProgressOnStopped;

        if (_currentProgress == progress)
        {
            _currentProgress = null;
        }
    }
    }
}