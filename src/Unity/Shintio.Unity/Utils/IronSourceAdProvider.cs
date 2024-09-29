// using System.Threading.Tasks;
// using Shintio.Unity.Enums;
// using Shintio.Unity.Interfaces;
// using UnityEngine;
// using UnityEngine.Advertisements;
//
// namespace Shintio.Unity.Utils
// {
//     public class IronSourceAdProvider : IAdProvider
//     {
//         private const string CoreId = "7971808895149";
//
//         private static bool _isInitialized = false;
//
//         private TaskCompletionSource<bool>? _initializeAdSource;
//         private TaskCompletionSource<bool>? _loadAdSource;
//         private TaskCompletionSource<bool>? _showAdSource;
//
//         public async Task<bool> LoadAd(AdType adType)
//         {
//             if (!await Initialize())
//             {
//                 return false;
//             }
//
//             _loadAdSource = new TaskCompletionSource<bool>();
//
//             Advertisement.Load(GetPlacementId(adType), this);
//
//             return await _loadAdSource.Task;
//         }
//
//         public async Task<bool> ShowAd(AdType adType)
//         {
//             if (!await Initialize())
//             {
//                 return false;
//             }
//
//             _showAdSource = new TaskCompletionSource<bool>();
//
//             Advertisement.Show(GetPlacementId(adType), this);
//
//             return await _showAdSource.Task;
//         }
//
//         public void OnUnityAdsAdLoaded(string adUnitId)
//         {
//             _loadAdSource?.TrySetResult(true);
//             _loadAdSource = null;
//         }
//
//         public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
//         {
//             _loadAdSource?.TrySetResult(false);
//             _loadAdSource = null;
//         }
//
//         public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
//         {
//             _showAdSource?.TrySetResult(true);
//             _showAdSource = null;
//         }
//
//         public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
//         {
//             _showAdSource?.TrySetResult(false);
//             _showAdSource = null;
//         }
//
//         public void OnUnityAdsShowStart(string adUnitId)
//         {
//         }
//
//         public void OnUnityAdsShowClick(string adUnitId)
//         {
//         }
//
//         public void OnInitializationComplete()
//         {
//             _initializeAdSource?.TrySetResult(true);
//             _initializeAdSource = null;
//         }
//
//         public void OnInitializationFailed(UnityAdsInitializationError error, string message)
//         {
//             _initializeAdSource?.TrySetResult(false);
//             _initializeAdSource = null;
//         }
//
//         private async Task<bool> Initialize()
//         {
//             if (_isInitialized)
//             {
//                 return true;
//             }
//             
//             IronSource.Agent.init(CoreId);
//             if (!Advertisement.isInitialized && Advertisement.isSupported)
//             {
//                 _initializeAdSource = new TaskCompletionSource<bool>();
//
//                 Advertisement.Initialize(AndroidGameId, true, this);
//
//                 if (!await _initializeAdSource.Task)
//                 {
//                     return false;
//                 }
//             }
//
//             _isInitialized = true;
//
//             return true;
//         }
//
//         private string GetPlacementId(AdType adType)
//         {
//             var platform = (Application.platform == RuntimePlatform.IPhonePlayer)
//                 ? "iOS"
//                 : "Android";
//
//             return $"{GetPlacementType(adType)}_{platform}";
//         }
//
//         private string GetPlacementType(AdType type) => type switch
//         {
//             AdType.Banner => "Banner",
//             AdType.Short => "Interstitial",
//             AdType.Long => "Rewarded",
//             _ => "Banner",
//         };
//     }
// }