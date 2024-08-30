using System.Threading.Tasks;
using Shintio.Essentials.Enums;

namespace Shintio.Essentials.Interfaces
{
    public interface IAdProvider
    {
        public Task<bool> LoadAd(AdType adType);
        public Task<bool> ShowAd(AdType adType);
    }
}