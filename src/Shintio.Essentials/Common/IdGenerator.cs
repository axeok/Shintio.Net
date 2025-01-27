namespace Shintio.Essentials.Common
{
	public class IdGenerator
	{
		private int _id = 0;
		
		public int GetNext()
		{
			return ++_id;
		}
	}
}