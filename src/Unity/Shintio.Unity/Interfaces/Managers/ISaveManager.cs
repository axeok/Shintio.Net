using System.Threading.Tasks;

namespace Shintio.Unity.Interfaces.Managers
{
    public interface ISaveManager
    {
        public Task Save();
        public Task Load();
    }
}