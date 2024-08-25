using Shintio.Essentials.Converters;
using Shintio.Essentials.Interfaces;
using Shintio.Json.Attributes;
using Shintio.Localization.Enums;

namespace Shintio.Localization.Interfaces
{
	[JsonConverter(typeof(HasDiscriminatorJsonConverter<IHasTranslation>), false)]
	public interface IHasTranslation : IHasDiscriminator
	{
		string Get(II18N i18N, Language language);
	}
}