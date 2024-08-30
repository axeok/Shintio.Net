using System;

namespace Shintio.Unity.Interfaces
{
    public interface IDebugger
    {
        public event Action<string> Logged;

        public void Log(string message);
    }
}