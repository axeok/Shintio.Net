using Shintio.Unity.Interfaces.Managers;
using UnityEngine;
using Zenject;

namespace Shintio.Unity.Ui.Buttons
{
    public class SaveOrLoadButton : MonoBehaviour
    {
        private ISaveManager _saveManager = null!;

        [Inject]
        public void Construct(ISaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public void Save()
        {
            _saveManager.Save();
        }

        public void Load()
        {
            _saveManager.Load();
        }
    }
}