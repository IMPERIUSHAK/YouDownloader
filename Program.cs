using System;
using YouDownloader.Bot;
using YouDownloader.User;

namespace YouDownloader;

class Program
{
    static void Main(string[] args)
    {
        var bot = new YouTubeBot();
        
        try
        {
            string videoUrl = UserUI.YouTubeLink();
            bot.DownloadVid(videoUrl);
        }
        catch (Exception ex)
        {
            UserUI.ShowError(ex.Message);
        }
        finally
        {
            UserUI.WaitForExit();
            bot.Close();
        }
    }
}