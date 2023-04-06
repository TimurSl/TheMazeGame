using Microsoft.Win32;
using static Microsoft.Win32.Registry;

namespace SimpleMazeGame;

public class StandartLevelSystem : LevelSystem
{
    public int GetLevel()
    {
        // get level in registry 
        if(Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            RegistryKey key = CurrentUser.OpenSubKey(@"Software\SimpleMazeGame");
            if (key == null)
            {
                key = CurrentUser.CreateSubKey(@"Software\SimpleMazeGame");
                key.SetValue("Level", 1);
            }
            
            return (int) key.GetValue("Level");
        }
        
        if (!File.Exists("level.txt"))
        {
            File.WriteAllText("level.txt", "1");
        }

        return int.Parse(File.ReadAllText("level.txt"));
    }

    public void LevelUp()
    {
        // increase level in registry
        if(Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            RegistryKey key = CurrentUser.OpenSubKey(@"Software\SimpleMazeGame", true);
            key.SetValue("Level", GetLevel() + 1);
        }
        else
        {
            File.WriteAllText("level.txt", (GetLevel() + 1).ToString());
        }
    }
}