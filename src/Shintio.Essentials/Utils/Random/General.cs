using System;
using Shintio.Essentials.Common;

namespace Shintio.Essentials.Utils.Random
{
    public partial class Random
    {
        public Color Color()
        {
            return new Color(Int(0, 255), Int(0, 255), Int(0, 255), 255);
        }
    }
}