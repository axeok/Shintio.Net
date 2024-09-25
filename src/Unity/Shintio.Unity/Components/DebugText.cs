// using System;
// using System.Collections.ObjectModel;
// using Shintio.Essentials.Common;
// using Shintio.Unity.Interfaces;
// using Zenject;
//
// namespace Shintio.Unity.Common
// {
//     public class DebugText : ReactiveText<string>
//     {
//         private const int MaxLines = 10;
//
//         private readonly ObservableCollection<string> _lines = new();
//         private readonly ReactiveProperty<string> _text = new("");
//
//         [Inject]
//         public void Construct(IDebugger debugger)
//         {
//             debugger.Logged += (message) =>
//             {
//                 if (_lines.Count >= MaxLines)
//                 {
//                     _lines.RemoveAt(0);
//                 }
//
//                 _lines.Add(message);
//             };
//
//             _lines.CollectionChanged += (_, _) => _text.Value = string.Join(Environment.NewLine, _lines);
//         }
//
//         protected override ReactiveProperty<string> GetReactiveProperty()
//         {
//             return _text;
//         }
//     }
// }