namespace Shintio.Essentials.Utils;

public static class FileSystem
{
    public static bool ValidateOrCreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }

        Directory.CreateDirectory(path);

        return false;
    }
}