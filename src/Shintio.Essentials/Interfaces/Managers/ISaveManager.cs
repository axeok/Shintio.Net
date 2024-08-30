using System.Threading.Tasks;

namespace Shintio.Essentials.Interfaces.Managers
{
    public interface ISaveManager
    {
        public Task Save();
        public Task Load();
    }
}