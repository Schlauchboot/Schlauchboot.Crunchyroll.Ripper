//Open Tasks: Implement custom Ffmpeg-Arguments, Error-Handling

using System;
using System.Threading.Tasks;

using FFMpegCore;

namespace Schlauchboot.Ripper.Crunchyroll.Manager.Methods
{
    /// <summary>
    /// Includes all Ffmpeg related Methods.
    /// </summary>
    public class Ffmpeg
    {
        
        /// <summary>
        /// Downloads an Episode based on a M3u8-File.
        /// </summary>
        /// <param name="episodeUrl">A Url corresponding to an M3u8-File.</param>
        /// <param name="targetFilePath">The full Path for the Output-File.</param>
        public async Task DownloadEpisode(Uri episodeUrl, string targetFilePath)
        {
            await FFMpegArguments.FromUrlInput(episodeUrl)
                .OutputToFile(targetFilePath, true, options => options
                    .WithCustomArgument("-bsf:a aac_adtstoasc")
                    .WithCustomArgument("-vcodec copy")
                    .WithCustomArgument("-c copy")
                    .WithCustomArgument("-crf 50")
                    .WithCustomArgument("-tune animation")
                    .UsingThreads(8)
                    .UsingMultithreading(true))
                .ProcessAsynchronously();
        }
    }

}
