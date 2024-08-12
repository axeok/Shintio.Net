using System;

namespace Shintio.Essentials.Utils.Random
{
    public partial class Random
    {
        private static Random? _instance;

        private readonly System.Random _random;

        public Random(int? seed = null)
        {
            _random = new System.Random(seed ?? (int)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
        }

        private static Random Instance => _instance ??= new Random();
    }
}