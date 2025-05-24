using System;
using System.Reflection.Metadata.Ecma335;
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

    private readonly string YouUrl="https://ssyoutube.com";
    public YouTubeBot()
    {
        var options = new ChromeOptions();
        options.AddArguments(
            "--headless=new",
            "--disable-gpu",
            "--window-size=1920,1080",
            "--no-sandbox",
            "--disable-dev-shm-usage",
            "--disable-extensions",
            "--disable-notifications"
        );

        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        _driver = new ChromeDriver(service, options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    public void CheckForVid(string YouLink)
    {
        try
        {
            _driver.Navigate().GoToUrl(YouUrl);

            var inputTxt = _wait.Until(d => d.FindElement(By.Id("id_url")));
            var sendLink = _wait.Until(d => d.FindElement(By.Id("search")));

            inputTxt.Clear();
            inputTxt.SendKeys(YouLink);
            sendLink.Click();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
    //улучшить
    public bool isQuality(IWebElement e)
    {
        if (string.IsNullOrEmpty(e.Text))
            return false;

        return char.IsDigit(e.Text[0]) &&
          e.Text.EndsWith(".mp4");
    }
    public int toNum(String str)
    {
        int flag = 0;
        foreach (char i in str)
        {
            if (i == '.') break;
            flag = flag * 10 + ((int)i - 48);
        }
        return flag;
    }


    public void Close()
    {
        _driver.Quit();
        _driver?.Dispose();
    }


    public List<string> GetAviableQualities()
    {
        var ParentEl = _wait.Until(d => d.FindElement(By.ClassName("table")));

        IReadOnlyCollection<IWebElement> vidQuality = _wait.Until(d => ParentEl.FindElements(By.TagName("span")));
        List<String> qualityList = new List<String>();

        foreach (IWebElement el in vidQuality)
        {
            if (isQuality(el))
            {
                qualityList.Add(el.Text);
                //Console.WriteLine($"{key}.{el.Text}");
            }
        }
        return qualityList;
    }

    public void ShowAviableQualities()
    {
        List<string> qualityList = GetAviableQualities();
        qualityList.Select((quality, index) => $"{index + 1}.{quality}")
          .ToList()
          .ForEach(Console.WriteLine);
    }
    public void ChangeTabs(Action action)
    {
        string firstTab = _driver.CurrentWindowHandle;

        try
        {
            action.Invoke();

            if (_driver.WindowHandles.Count > 1)
            {
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());
                _driver.Close();
            }
        }
        finally
        {
            _driver.SwitchTo().Window(firstTab);
        }
    }
    public bool isLinkChanged()
    {
        while (_driver.CurrentWindowHandle == YouUrl)
        {
            int t = 1, k = 0;
           
            Console.Write("Loading");
            while (k < t)
            {
                Console.Write(".");
                k++;
            }
            t++;
            if (t == 3) t = 1;
        }
        return true;
    }
    async public Task DownloadVid(int num)
    {
        try
        {
            List<string> qualityList = GetAviableQualities();
            var linkVid = _wait.Until(d => d.FindElement(By.Id($"download-mp4-{toNum(qualityList[num - 1])}-audio")));
            
            ChangeTabs(() => linkVid.Click());
            
            if (isLinkChanged())
            {
                IWebElement? videoElement = _wait.Until(d => d.FindElement(By.TagName("video")));
                
                string videoUrl = videoElement.GetAttribute("src");
                if (string.IsNullOrEmpty(videoUrl))
                    throw new Exception("wrong src URL");
                    
                if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out Uri videoUri))
                {
                   
                    videoUri = new Uri(new Uri(_driver.Url), videoUrl);
                }

                using (HttpClient client = new HttpClient())
                {
                    byte[] videoData = await client.GetByteArrayAsync(videoUri);
                    await File.WriteAllBytesAsync($"video2.mp4", videoData);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
    
}   
