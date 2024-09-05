namespace Shintio.Essentials.Common
{
    public partial class Color
    {
        public static readonly Color Transparent = new Color(0, 0, 0, 0);

        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color Black = new Color(0, 0, 0);

        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color Yellow = new Color(255, 255, 0);
        public static readonly Color LightGray = new Color(211, 211, 211);
        public static readonly Color Gray = new Color(143, 143, 143);
        public static readonly Color Orange = new Color(255, 165, 0);

        public static readonly Color MediumBlue = new Color(0, 0, 205);
        public static readonly Color LightBlue = new Color(0, 212, 252);
        public static readonly Color LighterBlue = new Color(173, 223, 255);

        // TODO: убрать в проект Роксланда
        public static readonly Color Roxland = new Color(49, 24, 148);

        public static readonly Color Error = new Color(255, 81, 38);
        public static readonly Color Warning = new Color(255, 213, 63);
        public static readonly Color Success = new Color(133, 193, 218);
        public static readonly Color Info = new Color(0, 193, 252);
    }
}