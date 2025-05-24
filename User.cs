using YouDownloader.Bot;

namespace  YouDownloader.User;

public static class UserUI
{
    public static string YouTubeLink()
    {
        Console.WriteLine("Link to YouTube Video:");
        return Console.ReadLine() ?? string.Empty;
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