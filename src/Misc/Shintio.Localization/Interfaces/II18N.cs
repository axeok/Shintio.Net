using System;
using System.Collections.Generic;
using Shintio.Localization.Enums;

namespace Shintio.Localization.Interfaces
{
	public interface II18N
	{
		string T(Language language, string key, params object[] args);
		string T(Language language, string key, IEnumerable<KeyValuePair<string, object>> args);
		string Counted(Language language, string key, uint number);
		string FormatTimespan(Language language, TimeSpan timeSpan);
	}
}