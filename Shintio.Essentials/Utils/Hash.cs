using System.Text;

namespace Shintio.Essentials.Utils;

public static class Hash
{
    public static uint Joaat(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return 0;
        byte[] bytes = Encoding.UTF8.GetBytes(data.ToLower().ToCharArray());
        uint num1 = 0;
        int index = 0;
        for (int length = bytes.Length; index < length; ++index)
        {
            uint num2 = num1 + (uint)bytes[index];
            uint num3 = num2 + (num2 << 10);
            num1 = num3 ^ num3 >> 6;
        }

        uint num4 = num1 + (num1 << 3);
        uint num5 = num4 ^ num4 >> 11;
        return num5 + (num5 << 15);
    }
}