# Simple Enigma2 / EMBY proxy for LiveTV
The service resides in between Enigma2 box (VU+ etc.) and EMBY media server. It emulates HDHomerun and provides with XMLTV program data (EPG data is provided by Enigma2 box).

## Prerequisites
- Enigma2 box with HR-Tuner Proxy plugin installed
- Windows or Linux box with .NET 5.0 (AspNetCore) installed. It could be the server running EMBY itself. How to install: https://docs.microsoft.com/en-us/dotnet/core/install/linux, choose aspnetcore-runtime-5.0

## Installation
- Copy ZIP file to the Linux box. For example: `pscp build/build.zip root@emby.box.local:/root/`
- Extract it: `unzip build.zip -d /root/EnigmaLiveTV`
- **Important**: modify `/root/EnigmaLiveTV/appsettings.json`
  - Change `ExternalUrl` to the proper address of the service
  - Change `HRProxyUrl` to the address of HR-Tuner Proxy plugin
  - Change `EPGUrl` to the correct STB address and bouquet (if needed)
  - Change/add `CCTVChannels` items
- Register as a service: 
  - `cp EnigmaLiveTV.service /etc/systemd/system/EnigmaLiveTV.service`
  - `systemctl daemon-reload`
  - `systemctl start EnigmaLiveTV`
  - `systemctl enable EnigmaLiveTV`
- Configure EMBY (LiveTV):
  - HDHomerun -> http://emby.box.local:5000
  - XMLTV -> http://emby.box.local:5000/epg.xml

## Disclaimer
This service is provided AS IS, without any obligation to support it in any manner.