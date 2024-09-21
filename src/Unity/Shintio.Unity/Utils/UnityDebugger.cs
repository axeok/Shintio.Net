using System;
using Shintio.Unity.Interfaces;
using UnityEngine;

namespace Shintio.Unity.Utils
{
    public class UnityDebugger : IDebugger
    {
        public event Action<string>? Logged;

        public void Log(string message)
        {
            Debug.Log($"[DEBUGGER] {message}");
            Logged?.Invoke(message);
        }
    }
}