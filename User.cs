using OpenQA.Selenium.BiDi.Modules.Input;
using YouDownloader.Bot;

namespace  YouDownloader.User;

public static class UserUI
{
    public static string YouTubeLink()
    {
        Console.WriteLine("Link to YouTube Video:");
        return Console.ReadLine() ?? string.Empty;
    }

    public static int PickQuality()
    {
        Console.WriteLine("Pick your the quality of video you want to install(default-1):");
        string? input = Console.ReadLine();
        int Ukey = string.IsNullOrEmpty(input) ? 1 : int.Parse(input);
        return Ukey;
    }
    public static void ShowError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public static void WaitForExit()
    {
        Console.WriteLine("Esc to quit...");
        Console.ReadLine();
    }
}