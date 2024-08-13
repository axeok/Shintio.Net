using System.Threading.Tasks;
using Shintio.Essentials.Utils;

namespace Shintio.Essentials.Interfaces
{
	public interface IOutput
	{
		public Task Write(string message);
		public Task WriteLine(string message);

		public Task<OutputProgress> CreateProgress(string title, double max);
	}
}