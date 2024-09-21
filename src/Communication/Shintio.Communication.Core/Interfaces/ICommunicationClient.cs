using System;
using System.Threading.Tasks;

namespace Shintio.Communication.Core.Interfaces
{
	public interface ICommunicationClient : ICommunicationUnit
	{
		public event Action Connected;

		public Task Connect();
	}
}