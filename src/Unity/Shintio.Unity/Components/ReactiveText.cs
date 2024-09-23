using Shintio.Essentials.Common;
using TMPro;
using UnityEngine;

namespace Shintio.Unity.Common
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class ReactiveText<T> : MonoBehaviour
    {
        private TextMeshProUGUI _text = null!;

        protected abstract ReactiveProperty<T> GetReactiveProperty();

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();

            var property = GetReactiveProperty();

            property.Changed += UpdateText;
            UpdateText(property.Value);
        }

        protected virtual string FormatText(T value)
        {
            return value?.ToString() ?? "";
        }

        private void UpdateText(T value)
        {
            _text.text = FormatText(value);
        }
    }
}