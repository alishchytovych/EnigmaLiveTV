# Simple Enigma2 proxy for PLEX, EMBY, Jellifin LiveTV
The service resides in between Enigma2 box (VU+ etc.) and EMBY/PLEX/Jellyfin media server. It emulates HDHomerun and provides with XMLTV program data (EPG data and picons are provided by the Enigma2 box).

## Prerequisites
- Enigma2 (OpenPLi, ViX etc.) satellite receiver with OpenWebIf enabled
- Windows or Linux box with .NET 5.0 (AspNetCore) installed. It could be the server running EMBY/PLEX/Jellyfin itself. How to install: https://docs.microsoft.com/en-us/dotnet/core/install/linux, choose aspnetcore-runtime-5.0

## Installation
- Copy ZIP file to the Linux box. For example: `pscp build/build.zip root@emby.box.local:/root/`
- Extract it: `unzip build.zip -d /root/EnigmaLiveTV`
- **Important** to modify `/root/EnigmaLiveTV/appsettings.json` section `EnigmaLiveTV`:
  - Change `STB` to the proper URI of the satellite box. No auth is supported.
  - Change `StreamingPort` to the proper port
  - Change `Filter` to your bouquete name etc.
  - Change `AdditionalChannels` to the other streaming devices (CCTV, IPTV etc.). Note - PLEX is unable to play RTSP streams directly, maybe I'll implement transcoding support later.
- Register as a service: 
  - `cp EnigmaLiveTV/EnigmaLiveTV.service /etc/systemd/system/EnigmaLiveTV.service`
  - `systemctl daemon-reload`
  - `systemctl start EnigmaLiveTV`
  - `systemctl enable EnigmaLiveTV`
- Enable the port in the firewall (if needed). Fedora example:
  - `firewall-cmd --permanent --add-port=5000/tcp`
  - `firewall-cmd --reload`
- Configure EMBY (LiveTV):
  - HDHomerun -> http://emby.box.local:5000
  - XMLTV -> http://emby.box.local:5000/xmltv

Note: if you have multiple sat boxes - you can run multiple services on the different ports (see `appsettings.json` section `Kestrel`)

## Disclaimer
This service is provided AS IS, without any obligation to support it in any manner.
