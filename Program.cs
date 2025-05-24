using System;
using YouDownloader.Bot;
using YouDownloader.User;

namespace YouDownloader;

class Program
{
    static async Task Main(string[] args)
    {
        var bot = new YouTubeBot();

        try
        {
            string videoUrl = UserUI.YouTubeLink();
            bot.CheckForVid(videoUrl);
            bot.ShowAviableQualities();
            int num = UserUI.PickQuality();
            await bot.DownloadVid(num);
        }
        catch (Exception ex)
        {
            UserUI.ShowError(ex.Message);
        }
        finally
        {
            UserUI.WaitForExit();
        }
    }
}