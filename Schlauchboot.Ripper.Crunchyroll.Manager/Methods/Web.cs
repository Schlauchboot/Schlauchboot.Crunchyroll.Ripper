//Open Tasks: Find a Way to use the headless Browser

using System.Threading.Tasks;
using System.Collections.Generic;

using PuppeteerSharp;

namespace Schlauchboot.Ripper.Crunchyroll.Manager.Methods
{
    public class Web
    {
        /// <summary>
        /// Fetches a Browser (Downloads one if no one is present).
        /// </summary>
        /// <returns>The Revision of the fetched Browser.</returns>
        public async Task<RevisionInfo> GetBrowser()
        {
            return await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        }

        /// <summary>
        /// Fetches the Html for an Episode-Page.
        /// </summary>
        /// <param name="browserExecutablePath">The Path to a fetched Browser.</param>
        /// <param name="webPage">The Url to an Episode-Page.</param>
        /// <returns>The Html of the Page in Form of a String.</returns>
        public async Task<string> GetWebPage(string browserExecutablePath, string webPage)
        {
            var browser = await CreateBrowser(browserExecutablePath);
            var openPages = await browser.PagesAsync();
            await openPages[0].GoToAsync(webPage);
            var pageContent = await openPages[0].GetContentAsync();
            await browser.CloseAsync();
            return pageContent;
        }

        /// <summary>
        /// Fetches the Html for an Episode-Page.
        /// </summary>
        /// <param name="browserExecutablePath">The Path to a fetched Browser.</param>
        /// <param name="webPage">The Url to an Episode-Page.</param>
        /// <param name="cookie">A KeyValuePair which includes a Cookie Name/Value which will be inserted in the Browser.</param>
        /// <returns>The Html of the Page in Form of a String.</returns>
        public async Task<string> GetWebPage(string browserExecutablePath, string webPage, KeyValuePair<string, string> cookie)
        {
            var browser = await CreateBrowser(browserExecutablePath);
            var openPages = await browser.PagesAsync();
            await openPages[0].GoToAsync(webPage);
            await openPages[0].DeleteCookieAsync();
            await openPages[0].SetCookieAsync(new CookieParam()
            {
                Domain = ".crunchyroll.com",
                Name = cookie.Key,
                Value = cookie.Value,
                HttpOnly = true
            });
            await openPages[0].ReloadAsync();
            var pageContent = await browser.PagesAsync().Result[0].GetContentAsync();
            await browser.CloseAsync();
            return pageContent;
        }

        /// <summary>
        /// Creates and Launches a Browser.
        /// </summary>
        /// <param name="browserExecutablePath">Path to the executable File of the Browser.</param>
        /// <returns>A Task for a Browser-Object.</returns>
        private async Task<Browser> CreateBrowser(string browserExecutablePath)
        {
            return await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = browserExecutablePath,
                Headless = false,
                Args = new string[] { "--window-position=0,0", "--window-size=1,1" }
            });
        }
    }
}
