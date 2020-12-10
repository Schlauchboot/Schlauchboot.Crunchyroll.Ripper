# Schlauchboot.Crunchyroll.Ripper
A .Net Core based Console-Application to download Episodes from Crunchyroll.

## Introduction
This Project aims to provide a more Automation-friendly Way to download Episodes from Crunchyroll. Therefore, a second Project is included in this Repository ([Schlauchboot.Crunchyroll.Ripper.Manager](https://github.com/Schlauchboot/Schlauchboot.Crunchyroll.Ripper/tree/master/Schlauchboot.Ripper.Crunchyroll.Manager)), which can be imported used for further use in, for example, Worker-Services. If you prefer a more polished GUI-Version, check out [Crunchyroll-Downloader-v3.0](https://github.com/hama3254/Crunchyroll-Downloader-v3.0).

## Warning
The Software provided here does not come with any Waranty. Additionally, many Parts are still under Development, which means that not all Features are currently fully functional.

## Currently functioning Elements
- [X] Download single Episodes
- [X] Select Streams based on a JSON-Config
- [X] Download Premium Episodes (if you have an Account)
- [X] Download based on an Input-File

## Planed Features
- [ ] Download entire Seasons
- [ ] Let the User pass custom Ffmpeg-Arguments
- [ ] Implement a headless Browser

## A Breakdown of the Procedure
1. An Instance of Chromium will be started in GUI-Mode to bypass Cloudflare
2. The entered Episode-Page will be opened to get its Contents as a String
3. The String will be checked for various Information
4. The User will be prompted select an Episode if no Config is present

## Requirements
For this Project to work, you have to meet the following Software-Requirements:

- Windows 8/10 as your OS
- Ffmpeg available as a PATH-Variable ([Guide](https://video.stackexchange.com/questions/20495/how-do-i-set-up-and-use-ffmpeg-in-windows))
- .Net Core Runtime 3.1 installed ([Download](https://dotnet.microsoft.com/download/dotnet-core/3.1))

## Usage
To download a single Episode, you can input the following command in a Shell of your Choice:

```powershell
.\Schlauchboot.Ripper.Crunchyroll episode "https://www.crunchyroll.com/de/love-live-nijigasaki-high-school-idol-club/episode-7-haruka-kanata-and-beyond-798656"
```

To download a premium Episode, you can issue the following Command which utilises your Crunchyroll session_id (This ID can be found via the Chrome Developer-Tools under Application -> Cookies). This can also be used to bypass Region-Restrictions via a VPN, as Crunchyroll just checks the Region of your session_id, instead of your actual IP.

```powershell
.\Schlauchboot.Ripper.Crunchyroll episode "https://www.crunchyroll.com/de/love-live-nijigasaki-high-school-idol-club/episode-7-haruka-kanata-and-beyond-798656" "YOUR_SESSION_ID"
```

If you want to download a List of Episodes, you can pass a Text-File containing Episode-Links, instead of passing the Episode-Link directly. You can also pass a session_id here.

```powershell
.\Schlauchboot.Ripper.Crunchyroll file "PATH\TO\TEXT\FILE"
```

Every further necessary Input will be requested by the Console. However, a JSON-File can be configured beforehand, which will Skip the whole User-Selection. The Config-Template-File will be generated in in the same Folder as the Executable and needs to be present there. The following Scheme should give a quick Overview of all possible Parameters.

```json
{
  "preferedSubLang":"enEN",
  "preferedAudioLang":"jaJP",
  "outputPath":"C:\\Temp",
  "fallback":"false"
}
```

## Open Tasks
- Implement Error-Handling across the entire Project
