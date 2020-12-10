//Open Tasks: Implement Playlist-Support

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Schlauchboot.Ripper.Crunchyroll.Models;
using Schlauchboot.Ripper.Crunchyroll.Methods;
using Schlauchboot.Ripper.Crunchyroll.Manager.Methods;

namespace Schlauchboot.Ripper.Crunchyroll
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Config"))
            {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}\\Config");
            }
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Downloads"))
            {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}\\Downloads");
            }
            var setupManager = new Setup();
            Config config = null;
            if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Config\\config.json"))
            {
                setupManager.CreateConfigTemplate();
            }
            else
            {
                config = setupManager.ReadConfig();
            }
            if (args.Length < 2)
            {
                Console.WriteLine("\nPlease pass at least two Arguments to this Programm (inputType, inputPath)!\n");
                Environment.Exit(1);
            }
            var webManager = new Web();
            var browserRevision = await webManager.GetBrowser();
            var webPages = new List<string>();
            var parsingManager = new Parsing();
            if (args[0] == "file")
            {
                var episodeList = File.ReadAllLines(args[1]);
                foreach (var episode in episodeList)
                {
                    if (episode.Contains("http"))
                    {
                        webPages.Add(await webManager.GetWebPage(browserRevision.ExecutablePath, episode));
                    }
                }
            }
            else if (args[0] == "episode")
            {
                if (args.Length !>= 2)
                {
                    webPages.Add(await webManager.GetWebPage(browserRevision.ExecutablePath, args[1]));
                }
                else
                {
                    webPages.Add(await webManager.GetWebPage(browserRevision.ExecutablePath, args[1],
                        new KeyValuePair<string, string>("session_id", args[2])));
                }
            }
            else if (args[0] == "playlist")
            {
                throw new NotImplementedException();
            }
            foreach (var webPage in webPages)
            {
                var htmlDocument = parsingManager.generateHtmlDocument(webPage);
                var streamList = parsingManager.parseEpisodeHtmlM3u8(htmlDocument);
                streamList.RemoveAll(x => x.format != "adaptive_hls");
                var showTitle = parsingManager.parseEpisodeHtmlShow(htmlDocument);
                var episodeTitle = parsingManager.parseEpisodeHtmlTitle(htmlDocument);
                var episodeNumber = parsingManager.parseEpisodeHtmlNumber(htmlDocument);
                Console.WriteLine($"\nThe following Streams have been found for {episodeTitle}:\n");
                if (streamList.Count != 0)
                {
                    foreach (var stream in streamList)
                    {
                        Console.WriteLine("AudioLanguage: " + stream.audio_lang);
                        Console.WriteLine("SubLanguage: " + stream.hardsub_lang);
                        Console.WriteLine("M3u8: " + stream.url);
                        Console.WriteLine();
                    }
                    var audioSelection = String.Empty;
                    if (config != null && config.preferedAudioLang != String.Empty)
                    {
                        audioSelection = config.preferedAudioLang;
                    }
                    else
                    {
                        Console.Write("Which AudioLanguage should be chosen?: ");
                        audioSelection = Console.ReadLine();
                        Console.WriteLine();
                    }
                    var subSelection = String.Empty;
                    if (config != null && config.preferedSubLang != String.Empty)
                    {
                        subSelection = config.preferedSubLang;
                    }
                    else
                    {
                        Console.Write("Which SubLanguage should be chosen (null = No Sub)?: ");
                        subSelection = Console.ReadLine();
                        if (subSelection == "null")
                        {
                            subSelection = null;
                        }
                        Console.WriteLine();
                    }
                    var streamSelection = streamList.Where(x => x.audio_lang == audioSelection && x.hardsub_lang == subSelection);
                    var fallback = true;
                    if (streamSelection.Count() == 0)
                    {
                        Console.WriteLine("No Stream matches the Selection!\n");
                        if (config != null && config.fallback == false)
                        {
                            Console.WriteLine("Episode will be skipped!\n");
                            fallback = false;
                        }
                        else
                        {
                            Console.WriteLine("A random Stream will be selected!\n");
                            streamSelection = streamList.OrderBy(x => Guid.NewGuid());
                            fallback = true;
                        }
                    }
                    if (fallback)
                    {
                        Console.WriteLine("Your Episode will be downloaded!\n");
                        var ffmpegManager = new Ffmpeg();
                        if (config != null && config.outputPath != String.Empty)
                        {
                            await ffmpegManager.DownloadEpisode(new Uri(streamSelection.First().url),
                                $"{config.outputPath}\\{showTitle} - {episodeNumber} - {episodeTitle}.mp4");
                        }
                        else
                        {
                            await ffmpegManager.DownloadEpisode(new Uri(streamSelection.First().url),
                                $"{AppDomain.CurrentDomain.BaseDirectory}\\Downloads\\" +
                                    $"{showTitle} - {episodeNumber} - {episodeTitle}.mp4");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No downloadable Streams have been found!\n");
                }
            }
        }
    }
}
