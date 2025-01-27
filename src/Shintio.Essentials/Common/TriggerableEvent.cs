using System;

namespace Shintio.Essentials.Common
{
	public delegate void EventHandler<in TSender, in TArgs>(TSender sender, TArgs args);
	
	public class TriggerableEvent<TSender, TArgs>
	{
		public event EventHandler<TSender, TArgs>? Triggered;
    
		public void Trigger(TSender sender, TArgs args)
		{
			Triggered?.Invoke(sender, args);
		}
	}
}