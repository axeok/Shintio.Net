using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shintio.Unity.Enums;
using Shintio.Unity.Interfaces;
using Shintio.Unity.Utils;
using TMPro;
using UnityEngine;

namespace Shintio.Unity.Components.Ui.Buttons
{
    public class ShowAdButton : MonoBehaviour
    {
        public TextMeshProUGUI? Text;
        public TextMeshProUGUI? Loading;

        private readonly List<Type> _providerTypes = new List<Type>
        {
            // typeof(UnityAdProvider)
        };

        private void Start()
        {
        }

        public void OnClickShowAd()
        {
            // Debug.Log(IronSource.Agent.isRewardedVideoAvailable());
            
            // IronSource.Agent.showRewardedVideo();
            
            
            
            return;
            
            
            
            // ToggleLoading(true);
            //
            // var adProvider = await GetAdProvider(AdType.Long);
            // if (adProvider == null)
            // {
            //     Debug.LogWarning("No ad provider available.");
            //     ToggleLoading(false);
            //     return;
            // }
            //
            // await adProvider.ShowAd(AdType.Long);
            //
            // ToggleLoading(false);
        }

        private async Task<IAdProvider?> GetAdProvider(AdType adType)
        {
            foreach (var type in _providerTypes)
            {
                if (Activator.CreateInstance(type) is not IAdProvider provider)
                {
                    continue;
                }

                var loaded = await provider.LoadAd(adType);
                if (loaded)
                {
                    return provider;
                }
            }

            return null;
        }

        private void ToggleLoading(bool loading)
        {
            Loading?.gameObject.SetActive(loading);
            Text?.gameObject.SetActive(!loading);
        }
    }
}