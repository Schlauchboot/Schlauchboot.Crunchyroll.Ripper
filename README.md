# Schlauchboot.Crunchyroll.Ripper
A .Net-Core based Console-Application to download Episodes from Crunchyroll.

## Introduction
This Project aims to provide a more Automation-friendly Way to download Episodes from Crunchyroll. Therefore, a second Project is included in this Repository ([Schlauchboot.Crunchyroll.Ripper.Manager](https://github.com/Schlauchboot/Schlauchboot.Crunchyroll.Ripper/tree/master/Schlauchboot.Ripper.Crunchyroll.Manager)), which can be imported in other Projects for further use in Projects like Worker-Services.

## Warning
The Software provided here does not come with any Waranty. Additionally, some Parts are still under development, which means that not all Features are currently fully functional.

## Currently functioning Elements
- [X] Download single Episodes
- [X] Select Streams based on Config

## Planed Features
- [ ] Download entire Seasons
- [ ] Download based on an Input-File
- [ ] Download Premium Videos
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
- Ffmpeg available as a PATH-Variable
- .Net-Core 3.1 installed

## Usage
To download a single Episode, you can input the following command in a Shell of your Choice:

```powershell
.\Schlauchboot.Ripper.Crunchyroll episode "https://www.crunchyroll.com/de/love-live-nijigasaki-high-school-idol-club/episode-7-haruka-kanata-and-beyond-798656"
```

## Open Tasks
- Cleanup the Code
