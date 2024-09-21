using System.Threading.Tasks;
using Shintio.Unity.Enums;

namespace Shintio.Unity.Interfaces
{
	public interface IAdProvider
	{
		public Task<bool> LoadAd(AdType adType);
		public Task<bool> ShowAd(AdType adType);
	}
}