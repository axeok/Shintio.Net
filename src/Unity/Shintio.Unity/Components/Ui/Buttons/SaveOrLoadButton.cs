using Shintio.Unity.Interfaces.Managers;
using UnityEngine;

namespace Shintio.Unity.Components.Ui.Buttons
{
    public class SaveOrLoadButton : MonoBehaviour
    {
        [SerializeField] private GameObject SaveManager = null!;
        
        private ISaveManager _saveManager = null!;

        private void Awake()
        {
            _saveManager = SaveManager.GetComponent<ISaveManager>();
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