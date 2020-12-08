//Open Tasks: Error-Handling

using System.Linq;
using System.Collections.Generic;

using HtmlAgilityPack;
using Newtonsoft.Json;

using Schlauchboot.Ripper.Crunchyroll.Manager.Models;

namespace Schlauchboot.Ripper.Crunchyroll.Manager.Methods
{
    /// <summary>
    /// All Methods need a HtmlDocument.
    /// </summary>
    public class Parsing
    {
        /// <summary>
        /// Generates a HtmlDocument from a String.
        /// </summary>
        /// <param name="htmlString">Html in Form of a String.</param>
        /// <returns>A HtmlDocument based on the Inputstring.</returns>
        public HtmlDocument generateHtmlDocument(string htmlString)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);
            return htmlDocument;
        }

        /// <summary>
        /// Parses a HtmlDocument for the Streams of an Episode.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>A List of all Streams for an Episode.</returns>
        public List<Stream> parseEpisodeHtmlM3u8(HtmlDocument htmlDocument)
        {
            var sourceHtml = htmlDocument.DocumentNode.SelectNodes("//*[contains(., 'vilos.config.media')]")[0].InnerHtml;
            string json = sourceHtml.Substring(sourceHtml.IndexOf("streams") + "streams".Length);
            int jsonEndpoint = json.IndexOf("ad_breaks") - 2;
            if (jsonEndpoint > 0) { json = json.Substring(0, jsonEndpoint); }
            return JsonConvert.DeserializeObject<List<Stream>>(json.Remove(0, 2).Replace("\\/", "/"));
        }

        /// <summary>
        /// Parses the Show-Title from an HtmlDocument.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>The Show-Title as String.</returns>
        public string parseEpisodeHtmlShow(HtmlDocument htmlDocument)
        {
            return htmlDocument.GetElementbyId("showmedia_about_episode_num").Descendants().ToList()[1].InnerText;
        }

        /// <summary>
        /// Parses the Episode-Title from an HtmlDocument.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>The Episode-Title as String.</returns>
        public string parseEpisodeHtmlTitle(HtmlDocument htmlDocument)
        {
            return htmlDocument.GetElementbyId("showmedia_about_name").InnerHtml.Replace('"', '\0');
        }
    }
}
