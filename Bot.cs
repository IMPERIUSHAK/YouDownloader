using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V135.Page;
using OpenQA.Selenium.Support.UI;

namespace YouDownloader.Bot;

public class YouTubeBot
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public YouTubeBot()
    {
        _driver = new ChromeDriver();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
    }
    public void DownloadVid(string YouLink)
    {
        try
        {
            _driver.Navigate().GoToUrl("https://ssyoutube.com");

            var InputTxt = _wait.Until(d => d.FindElement(By.Id("id_url")));
            var SendLink = _wait.Until(d => d.FindElement(By.Id("search")));

            InputTxt.Clear();
            InputTxt.SendKeys(YouLink);
            SendLink.Click();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public void Close()
    {
        _driver.Quit();
    }
}