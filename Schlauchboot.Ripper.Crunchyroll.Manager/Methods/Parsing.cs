//Open Tasks: Error-Handling

using System;
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
            try
            {
                htmlDocument.LoadHtml(htmlString);
                return htmlDocument;
            }
            catch (Exception)
            {
                return new HtmlDocument();
            }
        }

        /// <summary>
        /// Parses a HtmlDocument for the Streams of an Episode.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>A List of all Streams for an Episode.</returns>
        public List<Stream> parseEpisodeHtmlM3u8(HtmlDocument htmlDocument)
        {
            try
            {
                var sourceHtml = htmlDocument.DocumentNode.SelectNodes("//*[contains(., 'vilos.config.media')]")[0].InnerHtml;
                string json = sourceHtml.Substring(sourceHtml.IndexOf("streams") + "streams".Length);
                int jsonEndpoint = json.IndexOf("ad_breaks") - 2;
                if (jsonEndpoint > 0) { json = json.Substring(0, jsonEndpoint); }
                return JsonConvert.DeserializeObject<List<Stream>>(json.Remove(0, 2).Replace("\\/", "/"));
            }
            catch (Exception)
            {
                return new List<Stream>();
            }
        }

        /// <summary>
        /// Parses the Show-Title from an HtmlDocument.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>The Show-Title as String.</returns>
        public string parseEpisodeHtmlShow(HtmlDocument htmlDocument)
        {
            try
            {
                return htmlDocument.GetElementbyId("showmedia_about_episode_num").Descendants().ToList()[1].InnerText;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Parses the Episode-Title from an HtmlDocument.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>The Episode-Title as String.</returns>
        public string parseEpisodeHtmlTitle(HtmlDocument htmlDocument)
        {
            try
            {
                var episodeTitleString = htmlDocument.GetElementbyId("showmedia_about_name").InnerHtml.Substring(1);
                return episodeTitleString.Remove(episodeTitleString.Length - 1);
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Parses the Episode-Number from an HtmlDocument.
        /// </summary>
        /// <param name="htmlDocument">A HtmlDocument which represents a Crunchyroll-Page.</param>
        /// <returns>The Episode-Number as String.</returns>
        public string parseEpisodeHtmlNumber(HtmlDocument htmlDocument)
        {
            try
            {
                var episodeNumberString = String.Concat(htmlDocument
                    .GetElementbyId("showmedia_about_media").ChildNodes[3].InnerText.Where(c => !Char.IsWhiteSpace(c)));
                return episodeNumberString.Insert(episodeNumberString.Length - 1, " ");
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
