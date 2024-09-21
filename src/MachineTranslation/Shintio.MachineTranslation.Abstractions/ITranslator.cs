using System.Threading.Tasks;

namespace Shintio.MachineTranslation.Abstractions
{
    public interface ITranslator
    {
        public Task<string?> TranslateAsync(string text, string fromLanguage, string toLanguage);
    }
}